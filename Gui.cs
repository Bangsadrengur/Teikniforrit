using System;
using Gtk;
using Cairo;

public class Gui
{
    // Teikniborð
    Window win;
    DrawingArea darea;
    delegate void DrawShape(Cairo.Context ctx, PointD start, PointD end);
    DrawShape painter;
    // IO
    IoHandler ioh;
    bool listenOnMouse;

    public Gui()
    {
        Application.Init();

        ioh = new IoHandler();
        win = new Window("Drawing lines");
        darea = new  DrawingArea();
        painter = new DrawShape(DrawLine);
        listenOnMouse = false;

        darea.AddEvents(
                (int)Gdk.EventMask.PointerMotionMask
                | (int)Gdk.EventMask.ButtonPressMask
                | (int)Gdk.EventMask.ButtonReleaseMask);


        win.Hidden += delegate {Application.Quit();};
        win.KeyPressEvent += onKeyboardPressed;
        darea.ExposeEvent += onDrawingAreaExposed;
        darea.ButtonPressEvent += onMouseClicked;
        darea.ButtonReleaseEvent += onMouseReleased;

        win.SetDefaultSize(500,500);

        win.Add(darea);

        win.ShowAll();

        Application.Run();
    }

    // Send coordinates to IO handler and resolve if to flush screen
    // and ignore the mouse release.
    void onMouseClicked(object obj, ButtonPressEventArgs args)
    {
        PointD innHnit = new PointD(args.Event.X, args.Event.Y);
        bool flush = ioh.handleMouseClicked(innHnit);
        listenOnMouse = true;
        System.Console.WriteLine("onMousePressed: X=" +innHnit.X+",Y="+innHnit.Y);
        Console.WriteLine(flush);
    }

    // Send coordinates to IO handler.
    public void onMouseReleased(object obj, ButtonReleaseEventArgs args)
    {
        PointD utHnit = new PointD(args.Event.X, args.Event.Y);
        ioh.handleMouseReleased(utHnit);
        System.Console.WriteLine("onMouseReleased: X=" + utHnit.X + " ,Y=" + utHnit.Y);
        darea.QueueDraw();
    }

    PointD onMouseMotion(object obj, ButtonReleaseEventArgs args)
    {
    }
        

    // Send keyboard input to IO handler for evaluation.
    public void onKeyboardPressed(object obj, KeyPressEventArgs args)
    {
        // IO.updateMode
        //win.Title = args.Event.KeyValue;
        //ioh.updateMode(args.Event.Key);
        setPainter(args.Event.Key, this);
    }

    // Gets what to draw next on drawing area and draws it.
    void onDrawingAreaExposed(object source, ExposeEventArgs args)
    {
        // Resolve what to draw next (line, rectangle, ellipse or flush).
        using (Cairo.Context ctx = Gdk.CairoHelper.Create(darea.GdkWindow))
        {
            painter(ctx, ioh.getStartPoint(), ioh.getEndPoint());
        }
    }

    // Sets painter to the desired mode, L/l sets to line drawings,
    // R/r sets to rectangle drawings and E/e sets to ellipse drawings.
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
