
public class CamShakeMath
{
	/// <summary>
	/// Remap a value from one range to another.
	/// </summary>
	public static float Remap(float value, float oldFrom, float oldTo, float newFrom, float newTo)
	{
		return (value - oldFrom) / (oldTo - oldFrom) * (newTo - newFrom) + (newFrom);
	}
}
