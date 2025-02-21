using System;

namespace ASD
{

    class ChangeMaking
    {

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// bez ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości amount ( czyli rzędu o(amount) )
        /// </remarks>
        public int? NoLimitsDynamic(int amount, int[] coins, out int[] change)
        {
            const int MAX = (int)(1e9 + 7);
            int m = coins.Length;
            int[] dp = new int[amount + 1]; // dp[x] = min. number of coins that sum to x
            int[] last = new int[amount + 1]; // last[x] = the index of the last coin used in dp[x]
            dp[0] = 0;
            for (int x = 1; x <= amount; x++)
            {
                dp[x] = MAX;
            }
            for (int x = 1; x <= amount; x++)
            {
                for (int j = 0; j < m; j++)
                {
                    int c = coins[j];
                    if (x >= c)
                    {
                        int maybe = dp[x - c] + 1;
                        if (maybe < dp[x])
                        {
                            dp[x] = maybe;
                            last[x] = j;
                        }
                    }
                }
            }
            if (dp[amount] == MAX)
            {
                change = null;
                return null;
            }

            change = new int[m];
            int rem = amount;
            while (rem > 0)
            {
                change[last[rem]]++;
                rem -= coins[last[rem]];
            }

            return dp[amount];
        }

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// z uwzględnieniem ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="limits">Liczba dostępnych monet danego nomimału</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// limits[i] - dostepna liczba monet i-tego rodzaju (nominału)
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości iloczynu amount*(liczba rodzajów monet)
        /// ( czyli rzędu o(amount*(liczba rodzajów monet)) )
        /// </remarks>
        public int? Dynamic(int amount, int[] coins, int[] limits, out int[] change)
        {
            const int MAX = (int)(1e9 + 7);
            int m = coins.Length;
            int[] dp = new int[amount + 1]; // dp[x] = min. number of coins that sum to x
            int[,] cnt = new int[amount + 1, m]; // cnt[x][j] = count of coin j when making sum x 
            dp[0] = 0;
            for (int x = 1; x <= amount; x++)
            {
                dp[x] = MAX;
            }

            for (int j = 0; j < m; j++)
            {
                int c = coins[j];
                for (int k = 1; k <= limits[j]; k++)
                {
                    for (int x = amount; x >= 0; x--)
                    {
                        if (x + c <= amount)
                        {
                            int maybe = dp[x] + 1;
                            if (maybe < dp[x + c])
                            {
                                dp[x + c] = maybe;
                                for (int i = 0; i < m; i++)
                                {
                                    cnt[x + c, i] = cnt[x, i];
                                }
                                cnt[x + c, j]++;
                            }
                        }
                    }
                }
            }
            if (dp[amount] == MAX)
            {
                change = null;
                return null;
            }

            change = new int[m];
            for (int i = 0; i < m; i++)
            {
                change[i] = cnt[amount, i];
            }
            return dp[amount];
        }
    }
}
