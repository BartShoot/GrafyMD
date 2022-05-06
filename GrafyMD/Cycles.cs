using System;
using System.Collections.Generic;
using System.Linq;

namespace GrafyMD
{
    public class Cycles
    {
        public List<int[]> cycle;
        public Cycles()
        {
            cycle = new List<int[]>();
        }
        internal bool Is3Cycle(int i, int j, int k)
        {
            foreach (int[] n in cycle)
            {
                if (n.Contains(i))
                {
                    if (n.Contains(j))
                    {
                        if (n.Contains(k))
                            return true;
                    }
                }
            }
            return false;
        }

        internal bool Is4Cycle(int i, int j, int k, int l)
        {
            foreach (int[] n in cycle)
            {
                if (n.Contains(i))
                {
                    if (n.Contains(j))
                    {
                        if (n.Contains(k))
                        {
                            if (n.Contains(l))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}