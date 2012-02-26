using System;
using Gtk;
using Cairo;

// Gui is a class that draws a simple drawing board on screen. It is possible
// to draw a line, a rectangle or an ellipse on the screen, one at a time and
// never more than one object at once. The size of each object is controlled
// by drawing between a position where the mouse is clicked to a position where
// the mouse resides while the mouse button has not yet been released.
// Switching between drawing lines, rectangles or ellipses is done through
// keyboard input where L/l changes to drawing lines, R/r changes to drawing
// rectangles and E/e changes to drawing ellipses. The title of the window
// reflects the set mode of the program.
public class Gui
{
    // Fastayrðing gagna:
    // win er gluggi, darea er teikniborð.
    // painter er breyta til
    // að halda utan um úthlutun mismunandi falla sem notuð eru
    // fyrir mismunandi teikniaðferðir.
    // ioh höndlar flesta úrvinnslu á inntökum.
    // listenOnMouse kveikir eða slekkur á því hvort skrá eigi
    // niður hreyfingar músar.

    // Teikniborð
    Window win;
    DrawingArea darea;
    delegate void DrawShape(Cairo.Context ctx, PointD start, PointD end);
    DrawShape painter;
    // IO
    IoHandler ioh;
    bool listenOnMouse;

    // Constructor
    public Gui()
    {
        Application.Init();

        ioh = new IoHandler();
        win = new Window("Drawing lines");
        darea = new  DrawingArea();
        painter = new DrawShape(DrawLine);
        listenOnMouse = false; // Við hlustum ekki á mús við núllstöðu.

        // Aukum viðburðasett teikniborðs með ,möskum'.
        darea.AddEvents(
                (int)Gdk.EventMask.PointerMotionMask
                | (int)Gdk.EventMask.ButtonPressMask
                | (int)Gdk.EventMask.ButtonReleaseMask);


        // Úthlutum virkni á viðburði.
        win.Hidden += delegate {Application.Quit();};
        win.KeyPressEvent += onKeyboardPressed;
        darea.ExposeEvent += onDrawingAreaExposed;
        darea.ButtonPressEvent += onMouseClicked;
        darea.ButtonReleaseEvent += onMouseReleased;
        darea.MotionNotifyEvent += onMouseMotion;

        // Grunnstillum stærð glugga.
        win.SetDefaultSize(500,500);

        // Lokasamantekt til að virkja glugga.
        win.Add(darea);
        win.ShowAll();
        Application.Run();
    }

    // Send coordinates to IO handler and resolve if to flush screen
    // and ignore mouse movements or to start drawing on screen and
    // watch mouse movements.
    void onMouseClicked(object obj, ButtonPressEventArgs args)
    {
        PointD innHnit = new PointD(args.Event.X, args.Event.Y);
        bool flush = ioh.handleMouseClicked(innHnit);
        if(flush)
        {
            win.QueueDraw();
        } else {
        listenOnMouse = true;
        }
        System.Console.WriteLine("onMousePressed: X=" +innHnit.X+",Y="+innHnit.Y);
        Console.WriteLine(flush);
    }

    // Seize watching for mouse movements.
    public void onMouseReleased(object obj, ButtonReleaseEventArgs args)
    {
        listenOnMouse = false;
    }

    // Updates the endpoint for which an object will be drawn to from
    // the starting point as long as we're listening for mouse movements.
    void onMouseMotion(object obj, MotionNotifyEventArgs args)
    {
        if(listenOnMouse)
        {
            ioh.handleMouseMotion(new PointD(args.Event.X, args.Event.Y));
            darea.QueueDraw();
        }
    }

    // Forward keyboard input to a function that sets the object to 
    // draw accordingly.
    public void onKeyboardPressed(object obj, KeyPressEventArgs args)
    {
        setPainter(args.Event.Key, this);
    }

    // Draws on screen what the object painter is set to paint.
    void onDrawingAreaExposed(object source, ExposeEventArgs args)
    {
        using (Cairo.Context ctx = Gdk.CairoHelper.Create(darea.GdkWindow))
        {
            painter(ctx, ioh.getStartPoint(), ioh.getEndPoint());
        }
    }

    // Sets painter to the desired mode according passed variable, 
    // L/l sets to line drawings, R/r sets to rectangle drawings 
    // and E/e sets to ellipse drawings.
    // Changes window title according to the set mode.
    static void setPainter(Gdk.Key keyboardInput, Gui gui)
    {
        switch(keyboardInput)
        {
            case Gdk.Key.l:
                gui.painter = new DrawShape(DrawLine);
                gui.win.Title = "Drawing lines";
                break;
            case Gdk.Key.L:
                gui.painter = new DrawShape(DrawLine);
                gui.win.Title = "Drawing lines";
                break;
            case Gdk.Key.r:
                gui.painter = new DrawShape(DrawRectangle);
                gui.win.Title = "Drawing rectangles";
                break;
            case Gdk.Key.R:
                gui.painter = new DrawShape(DrawRectangle);
                gui.win.Title = "Drawing rectangles";
                break;
            case Gdk.Key.e:
                gui.painter = new DrawShape(DrawEllipse);
                gui.win.Title = "Drawing ellipses";
                break;
            case Gdk.Key.E:
                gui.painter = new DrawShape(DrawEllipse);
                gui.win.Title = "Drawing ellipses";
                break;
            default:
                break;
        }
    }

    // Draws a line from start to end.
    public static void DrawLine(Cairo.Context ctx, PointD start, PointD end)
    {
        ctx.MoveTo(start);
        ctx.LineTo(end);
        ctx.Stroke();
    }

    // Draws a rectangle from start to end.
    public static void DrawRectangle(Cairo.Context ctx, PointD start, PointD end)
    {
        double width = Math.Abs(start.X - end.X);
        double height = Math.Abs(start.Y - end.Y);
        double x = Math.Min(start.X, end.X);
        double y = Math.Min(start.Y, end.Y);
        ctx.Rectangle(x, y, width, height);
        ctx.Stroke();
    }

    // Draws an ellipse from start to end.
    public static void DrawEllipse(Cairo.Context ctx, PointD start, PointD end)
    {
        double width = Math.Abs(start.X - end.X);
        double height = Math.Abs(start.Y - end.Y);
        double xcenter = start.X + (end.X - start.X) / 2.0;
        double ycenter = start.Y + (end.Y - start.Y) / 2.0;

        ctx.Save();
        ctx.Translate(xcenter, ycenter);
        ctx.Scale(width/2.0, height/2.0);
        ctx.Arc(0.0, 0.0, 1.0, 0.0, 2*Math.PI);
        ctx.Restore();
        ctx.Stroke();
    }
}
