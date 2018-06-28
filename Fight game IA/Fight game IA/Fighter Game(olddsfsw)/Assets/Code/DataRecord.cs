using System;
using System.Collections.Generic;
using System.Text;

class DataRecord
{
    public Dictionary<Movements, int> counts;
    public int total;

    public DataRecord()
    {
        total = 0;
        counts = new Dictionary<Movements, int>();
    }
}
