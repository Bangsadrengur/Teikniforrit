using System;
using Gtk;
using Cairo;

public class IoHandler 
{
    // flush says if screen should be flushed or drawn upon.
    // mode holds numbers for drawing styles
    //  1 = Line, 2 = Rectancle, 3 = Ellipse
    // coordinates holds starting point in x,y at coordinates[0..1]
    // and ending point in x,y at coordinates[2..3] for last drawn
    // area with mouse.
    bool flush;
    int mode;
    int[] coordinates;
    
    // Initializes ioHandler, mode set to line at start and a
    // line drawn between upper left corner and lower right corner.
    public IoHandler()
    {
        coordinates = new int[4];
        // coordinates set for cornered line.
        mode = 1;
    }

    // Get mode number: 1 stands for line, 2 for rectangle and 3
    // for an ellipse.
    public int getMode()
    {
        return mode;
    }

    // Translates a character from keyboard to and integer setting
    // for mode where input  ,L/l' sets to line (mode=1), 
    // ,R/r' sets to rectangle (mode=2) and ,E/e' sets to ellipse
    // (mode=3).
    public void updateMode(char keyboardInput)
    {
        System.Console.WriteLine("IO.updateMode not implemented");
    }

    // If last mouse click ended in an object drawn to screen,
    // set flush to true and return flush.
    // If last mouse click ended in a flush, set flush to false,
    // update coordinates[0..1] and return flush.
    public bool handleMouseClicked(int[] mousePosition)
    {
        if(flush)
        {
            flush = false;
            coordinates[0] = mousePosition[0];
            coordinates[1] = mousePosition[1];
            return flush;
        } else {
            flush = true;
            return flush;
        }
    }

    // Mouse has been pressed and the screen was not flushed.
    // Update coordinates[2..3].
    public void handleMouseReleased(int[] mousePosition)
    {
        coordinates[2] = mousePosition[0];
        coordinates[3] = mousePosition[1];
    }

    // Gets coordinates for the area of the objects to be drawn.
    public int[] getCoordinates()
    {
        return coordinates;
    }
}

public class Gui
{
    // Teikniborð
    Window win;
    DrawingArea darea;
    // IO

    public Gui()
    {
        Application.Init();

        win = new Window("Drawing lines");
        darea = new  DrawingArea();

        win.Add(darea);

        win.ShowAll();

        Application.Run();
    }
    // Send coordinates to IO handler and resolve if to flush screen
    // and ignore the mouse release.
    public void onMouseClicked()
    {
        // IO.handleMouseClicked
        // Check if flush
    }

    // Send coordinates to IO handler.
    public void onMouseReleased()
    {
        // IO.handleMouseReleased
    }

    // Send keyboard input to IO handler for evaluation.
    public void onKeyboardPressed()
    {
        // IO.updateMode
    }

    public static void DrawEllipse(CairoContext cts, PointD start, PointD end)
    {
        double width = Math.Abs(start.X - end.X);
        double height = Math.Abs(start.Y - end.Y);
        double xcenter = start.X + (end.X - start.X) / 2.0;
        double ycenter = start.Y + (end.Y - start.Y) / 2.0;

        ctx.Save();
        ctx.Translate(xcenter, ycenter);
        cts.Scale(width/2.0, height/2.0);
        cts.Arc(0.0, 0.0, 1.0, 0.0, 2*Math.PI);
        cts.Restore();
        ctx.Stroke();
    }
}

public class Head
{
    public static void Main()
    {
        Gui gui = new Gui();
    }
}
