using System;
using Gtk;
using Cairo;

// IoHandler is an input handling class for some of the needs of 
// the graphical gtk class Gui.
public class IoHandler 
{
    // Data decleration:
    // flush says if screen should be flushed or drawn upon.
    // mode holds char indicator for drawing styles
    //  l = Line, r = Rectancle, e = Ellipse
    // startPoint holds the origin point for drawing for an external
    // drawing class.
    // endPoint holds last updated endpoint for drawing for an 
    // external drawing class.
    bool flush;
    char mode;
    PointD startPoint;
    PointD endPoint;

    // Initializes ioHandler, mode set to line at start and a
    // line drawn between upper left corner and lower right corner.
    // ,Last setting' for flush was false since we last issued that
    // a line should be drawn.
    public IoHandler()
    {
        startPoint = new PointD(0,0);
        // Set according to Gui.cs design at 26.2.12.
        endPoint = new PointD(500,500);
        mode = 'l';
        flush = false;
    }

    // Get mode char: l stands for line, r for rectangle and e
    // for an ellipse.
    public char getMode()
    {
        return mode;
    }

    // If last mouse click ended in an object drawn to screen,
    // set flush to true and return flush and set drawing points
    // both to zero position causing a ,flush' on the drawing area
    // on next draw event.
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
    public void handleMouseMotion(PointD endPoint)
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
