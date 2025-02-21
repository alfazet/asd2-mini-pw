using System;
using System.Diagnostics;

namespace ASD
{
    class CrossoutChecker
    {
        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec x
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Jedyny znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x)
        {
            foreach (char[] pat in patterns)
            {
                if (pat.Length == 1 && pat[0] == x)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec xy
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Pierwszy znak szukanego wzorca</param>
        /// <param name="y">Drugi znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x, char y)
        {
            foreach (char[] pat in patterns)
            {
                if (pat.GetLength(0) == 2 && pat[0] == x && pat[1] == y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda sprawdza, czy podany ciąg znaków można sprowadzić do ciągu pustego przez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="crossoutsNumber">Minimalna liczba skreśleń gwarantująca sukces lub int.MaxValue, jeżeli się nie da</param>
        /// <returns></returns>
        public bool Erasable(char[] s, char[][] patterns, out int crossoutsNumber)
        {
            const int MAX = (int)(1e9 + 7);
            int n = s.Length;
            int[,] dp = new int[n, n]; // dp[i][j] = min. number of deletions to erase s[i..=j]
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    dp[i, j] = MAX;
                }
            }
            for (int i = 0; i < n; i++)
            {
                dp[i, i] = comparePattern(patterns, s[i]) ? 1 : MAX;
            }

            for (int len = 2; len <= n; len++)
            {
                for (int i = 0; i <= n - 1 - len + 1; i++)
                {
                    int j = i + len - 1;
                    int maybe = MAX;
                    for (int k = i; k < j; k++)
                    {
                        maybe = Math.Min(maybe, dp[i, k] + dp[k + 1, j]);
                    }
                    foreach (char[] p in patterns)
                    {
                        if (p.Length == 1)
                        {
                            if (s[i] == p[0])
                            {
                                maybe = Math.Min(maybe, dp[i + 1, j] + 1);
                            }
                            if (s[j] == p[0])
                            {
                                maybe = Math.Min(maybe, dp[i, j - 1] + 1);
                            }
                        }
                        else if (p.Length == 2)
                        {
                            if (s[i] == p[0] && s[j] == p[1])
                            {
                                maybe = Math.Min(maybe, dp[i + 1, j - 1] + 1);
                            }
                        }
                    }
                    dp[i, j] = Math.Min(dp[i, j], maybe);
                }
            }

            crossoutsNumber = dp[0, n - 1];
            if (crossoutsNumber >= MAX)
            {
                crossoutsNumber = int.MaxValue;
            }
            return crossoutsNumber != int.MaxValue;
        }

        /// <summary>
        /// Metoda sprawdza, jaka jest minimalna długość ciągu, który można uzyskać z podanego poprzez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <returns></returns>
        public int MinimumRemainder(char[] s, char[][] patterns)
        {
            const int MAX = (int)(1e9 + 7);
            int n = s.Length;
            int[,] dp = new int[n, n]; // dp[i][j] = min. length of the string remaining from s[i..=j]
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    dp[i, j] = j - i + 1;
                }
            }
            for (int i = 0; i < n; i++)
            {
                dp[i, i] = comparePattern(patterns, s[i]) ? 0 : 1;
            }

            for (int len = 2; len <= n; len++)
            {
                for (int i = 0; i <= n - 1 - len + 1; i++)
                {
                    int j = i + len - 1;
                    int maybe = len;
                    for (int k = i; k < j; k++)
                    {
                        maybe = Math.Min(maybe, dp[i, k] + dp[k + 1, j]);
                    }
                    foreach (char[] p in patterns)
                    {
                        if (p.Length == 1)
                        {
                            if (s[i] == p[0])
                            {
                                maybe = Math.Min(maybe, dp[i + 1, j]);
                            }
                            if (s[j] == p[0])
                            {
                                maybe = Math.Min(maybe, dp[i, j - 1]);
                            }
                        }
                        else if (p.Length == 2)
                        {
                            if (s[i] == p[0] && s[j] == p[1])
                            {
                                maybe = Math.Min(maybe, dp[i + 1, j - 1]);
                            }
                        }
                    }
                    dp[i, j] = Math.Min(dp[i, j], maybe);
                }
            }

            return dp[0, n - 1];
        }

        // można dopisać metody pomocnicze

    }
}
