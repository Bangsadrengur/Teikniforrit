using System;
using Gtk;
using Cairo;

public class IoHandler 
{
    // flush says if screen should be flushed or drawn upon.
    // mode holds char indicator for drawing styles
    //  l = Line, r = Rectancle, e = Ellipse
    // coordinates holds starting point in x,y at coordinates[0..1]
    // and ending point in x,y at coordinates[2..3] for last drawn
    // area with mouse.
    bool flush;
    char mode;
    PointD startPoint;
    PointD endPoint;

    // Initializes ioHandler, mode set to line at start and a
    // line drawn between upper left corner and lower right corner.
    public IoHandler()
    {
        startPoint = new PointD(0,0);
        endPoint = new PointD(500,500);
        // coordinates set for cornered line.
        mode = 'l';
        flush = false;
    }

    // Get mode number: 1 stands for line, 2 for rectangle and 3
    // for an ellipse.
    public char getMode()
    {
        return mode;
    }

    // Sets mode to a char corresponding to available modes,
    // L/l for line, R/r for rectangle or E/e for Ellipse.
    public void updateMode(Gdk.Key keyboardInput)
    {
        Console.WriteLine("Keypress: {0}", keyboardInput);
        switch(keyboardInput)
        {
            case Gdk.Key.l:
                mode = 'l';
                break;
            case Gdk.Key.L:
                mode = 'l';
                break;
            case Gdk.Key.r:
                mode = 'r';
                break;
            case Gdk.Key.R:
                mode = 'r';
                break;
            case Gdk.Key.e:
                mode = 'e';
                break;
            case Gdk.Key.E:
                mode = 'e';
                break;
            default:
                break;
        }
        Console.WriteLine("Mode is: {0}", mode);
    }

    // If last mouse click ended in an object drawn to screen,
    // set flush to true and return flush.
    // If last mouse click ended in a flush, set flush to false,
    // update startPoint and return flush.
    public bool handleMouseClicked(PointD startPoint)
    {
        if(flush)
        {
            flush = false;
            this.startPoint = startPoint;
            return flush;
        } else {
            flush = true;
            this.startPoint = new PointD(0,0);
            this.endPoint = new PointD(0,0);
            return flush;
        }
    }

    // Mouse has been pressed and the screen was not flushed.
    // Update endPoint
    public void handleMouseReleased(PointD endPoint)
    {
        if(!flush)
        {
            this.endPoint = endPoint;
        }
    }

    // Get starting point for the area of the objects to be drawn.
    public PointD getStartPoint()

    {
        return startPoint;
    }

    // Get starting point for the area of the objects to be drawn.
    public PointD getEndPoint()

    {
        return endPoint;
    }
}
