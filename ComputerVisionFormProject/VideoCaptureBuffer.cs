using System.Drawing.Imaging;

///<summary>
///    Represents a buffer for storing video frames captured by a camera device.
///</summary>
///<remarks>
///    This class manages a circular buffer of Bitmap objects to store video frames. It provides methods
///    for adding new frames to the buffer and retrieving frames from the buffer.
///</remarks>
///<seealso cref="System.Drawing.Bitmap"/>
///<seealso cref="System.Drawing.Graphics"/>
///<seealso cref="System.Drawing.Color"/>
public class VideoCaptureBuffer
{
    /// <summary>
    /// The buffer for storing video frames.
    /// </summary>
    protected Bitmap[] buffer;

    /// <summary>
    /// The current index in the buffer.
    /// </summary>
    protected int index = 0;

    /// <summary>
    /// Determines if the buffer is set to scale the images before storing them
    /// </summary>
    protected bool scalesImages;

    /// <summary>
    /// Gets the current index in the buffer.
    /// </summary>
    public int Index { get { return index; } protected set { index = value; } }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoCaptureBuffer"/> class with the specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The size of the buffer.</param>
    public VideoCaptureBuffer(int bufferSize, bool enableScaling = false)
    {
        InitializeBuffer(bufferSize);

        scalesImages = enableScaling;
    }

    /// <summary>
    /// Scales the specified bitmap to the desired width and height using color interpolation.
    /// </summary>
    /// <param name="b">The bitmap to scale.</param>
    /// <param name="x">The desired width of the scaled image.</param>
    /// <param name="y">The desired height of the scaled image.</param>
    public void Scale(Bitmap b, int x, int y)
    {
        // Create a new bitmap with the desired dimensions
        Bitmap scaledBitmap = new Bitmap(x, y);

        // Calculate scaling factors for width and height
        float scaleX = (float)b.Width / x;
        float scaleY = (float)b.Height / y;

        // Loop through each pixel in the scaled image
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // Calculate the corresponding pixel coordinates in the original image
                int originalX = (int)(i * scaleX);
                int originalY = (int)(j * scaleY);

                // Get the color of the pixel in the original image
                Color interpolatedColor = b.GetPixel(originalX, originalY);

                // Set the color of the corresponding pixel in the scaled image
                scaledBitmap.SetPixel(i, j, interpolatedColor);
            }
        }

        // Replace the original bitmap with the scaled bitmap
        //b.Dispose(); // Dispose the original bitmap to free up resources

        Bitmap bmp = scaledBitmap.Clone(new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height), PixelFormat.Format24bppRgb);

        b = bmp; // Update the reference to the original bitmap
    }


    /// <summary>
    /// Initializes the buffer with blank bitmaps.
    /// </summary>
    /// <param name="bufferSize">The size of the buffer.</param>
    private void InitializeBuffer(int bufferSize)
    {
        buffer = new Bitmap[bufferSize];

        // Populate buffer with blank bitmaps
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = CreateBlankBitmap();
        }
    }

    /// <summary>
    /// Adds a new video frame to the buffer.
    /// </summary>
    /// <param name="frame">The bitmap representing the video frame.</param>
    public void push(Bitmap frame)
    {
        if (frame != null)
        {
            if( scalesImages ) { Scale( frame , (int)( 640f / 2f ) , (int)( 480f / 2f ) ); }

            // Update buffer with the new frame
            buffer[index] = frame;

            // Increment index with wrapping
            index = (index + 1) % buffer.Length;
        }
    }

    /// <summary>
    /// Retrieves the most recently added video frame from the buffer.
    /// </summary>
    /// <returns>The bitmap representing the video frame.</returns>
    public Bitmap pop()
    {
        // Retrieve bitmap from buffer
        int val = (index - 1 + buffer.Length) % buffer.Length;
        return buffer[val];
    }

    /// <summary>
    /// Creates a new blank bitmap with default width and height.
    /// </summary>
    /// <returns>The blank bitmap.</returns>
    private Bitmap CreateBlankBitmap()
    {
        int width = 640; // Assuming width of the blank bitmap
        int height = 480; // Assuming height of the blank bitmap

        // Create a new blank bitmap
        Bitmap bitmap = new Bitmap(width, height);

        // Fill the bitmap with a blank color (e.g., white)
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
        }

        return bitmap;
    }
}
