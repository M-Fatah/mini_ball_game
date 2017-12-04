using System.Collections;
using UnityEngine;

public static class Utils
{
	public static T[] ShuffleArray<T>(T[] array, int seed)
	{
		System.Random prng = new System.Random(seed);

		for(int i = 0; i < array.Length - 1; i++)
		{
			int randomIndex = prng.Next(i, array.Length);

			T tempItem = array[i];
			array[i] = array[randomIndex];
			array[randomIndex] = tempItem;
		}

		return array;
	}

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
