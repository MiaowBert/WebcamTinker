/*
    Copyright 2024 Nathan Krone

   Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License (CC BY-NC 4.0).
   You may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://creativecommons.org/licenses/by-nc/4.0/

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using ComputerVisionFormProject;
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
    public bool scalesImages;

    /// <summary>
    /// Gets the current index in the buffer.
    /// </summary>
    public int Index { get { return index; } protected set { index = value; } }


    public int Size {  get ; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoCaptureBuffer"/> class with the specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The size of the buffer.</param>
    public VideoCaptureBuffer(int bufferSize, bool enableScaling = false)
    {
        Size = bufferSize;

        InitializeBuffer(bufferSize);

        scalesImages = enableScaling;
    }

    /// <summary>
    /// Scales the specified bitmap to the desired width and height using bilinear interpolation.
    /// </summary>
    /// <param name="b">The bitmap to scale.</param>
    /// <param name="x">The desired width of the scaled image.</param>
    /// <param name="y">The desired height of the scaled image.</param>
    public Bitmap ScaleBilinearInterpolate(Bitmap b, int x, int y)
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
                float originalX = i * scaleX;
                float originalY = j * scaleY;

                // Calculate the surrounding pixel coordinates
                int x1 = (int)originalX;
                int x2 = x1 + 1;
                int y1 = (int)originalY;
                int y2 = y1 + 1;

                // Clamp the coordinates to stay within the bounds of the original image
                x1 = Math.Max(0, Math.Min(b.Width - 1, x1));
                x2 = Math.Max(0, Math.Min(b.Width - 1, x2));
                y1 = Math.Max(0, Math.Min(b.Height - 1, y1));
                y2 = Math.Max(0, Math.Min(b.Height - 1, y2));

                // Calculate the fractional parts for interpolation
                float deltaX = originalX - x1;
                float deltaY = originalY - y1;

                // Get the colors of the surrounding pixels
                Color c1 = b.GetPixel(x1, y1);
                Color c2 = b.GetPixel(x2, y1);
                Color c3 = b.GetPixel(x1, y2);
                Color c4 = b.GetPixel(x2, y2);

                // Interpolate colors using bilinear interpolation
                float red = Interpolate(c1.R, c2.R, c3.R, c4.R, deltaX, deltaY);
                float green = Interpolate(c1.G, c2.G, c3.G, c4.G, deltaX, deltaY);
                float blue = Interpolate(c1.B, c2.B, c3.B, c4.B, deltaX, deltaY);

                // Set the color of the corresponding pixel in the scaled image
                scaledBitmap.SetPixel(i, j, Color.FromArgb((int)red, (int)green, (int)blue));
            }
        }

        return scaledBitmap;
    }

    // Helper function for bilinear interpolation
    private float Interpolate(float s1, float s2, float s3, float s4, float deltaX, float deltaY)
    {
        return s1 * (1 - deltaX) * (1 - deltaY) +
               s2 * deltaX * (1 - deltaY) +
               s3 * (1 - deltaX) * deltaY +
               s4 * deltaX * deltaY;
    }


    /// <summary>
    /// Scales the specified bitmap to the desired width and height using color interpolation.
    /// </summary>
    /// <param name="b">The bitmap to scale.</param>
    /// <param name="x">The desired width of the scaled image.</param>
    /// <param name="y">The desired height of the scaled image.</param>
    public Bitmap Scale(Bitmap b, int x, int y )
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

        //Bitmap bmp = scaledBitmap.Clone(new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height), PixelFormat.Format24bppRgb);

        return scaledBitmap;
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
            if( scalesImages ) { 

                buffer[index] = ( ScaleBilinearInterpolate( frame , (int)( 640f / Config.scaleX ), (int)( 480f / Config.scaleY) ) );

            } else {

            // Update buffer with the new frame
                buffer[index] = frame;

            }

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
