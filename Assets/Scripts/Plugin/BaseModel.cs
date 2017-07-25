using System;

public class BaseModel : IModel
{
    public void SetValues(params string[] values)
    {
        int i = 0;
        foreach (var p in GetType().GetProperties())
        {
            p.SetValue(this, values[i]);
            i++;
        }
    }
}