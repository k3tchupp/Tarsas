using System;
using System.Collections.Generic;
using System.IO;

class Tarsas
{
    static void Main(string[] args)
    {
        string[] osvenyek = File.ReadAllLines("osvenyek.txt");
        int[] dobasok = File.ReadAllText("dobasok.txt").Split().Select(int.Parse).ToArray();

        Console.WriteLine("2. feladat");
        Console.WriteLine($"A dobások száma: {dobasok.Length}");
        Console.WriteLine($"Az ösvények száma: {osvenyek.Length}");

        int leghosszabbIndex = 0;
        for (int i = 1; i < osvenyek.Length; i++)
        {
            if (osvenyek[i].Length > osvenyek[leghosszabbIndex].Length)
            {
                leghosszabbIndex = i;
            }
        }

        Console.WriteLine("\n3. feladat");
        Console.WriteLine($"Az egyik leghosszabb a(z) {leghosszabbIndex + 1}. ösvény, hossza: {osvenyek[leghosszabbIndex].Length}");

        Console.WriteLine("\n4. feladat");
        Console.Write("Adja meg egy ösvény sorszámát! ");
        int osvenySorszam = int.Parse(Console.ReadLine());
        Console.Write("Adja meg a játékosok számát! ");
        int jatekosokSzama = int.Parse(Console.ReadLine());

        var statisztika = osvenyek[osvenySorszam - 1].GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        Console.WriteLine("\n5. feladat");
        foreach (var kvp in statisztika)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value} darab");
        }

        Console.WriteLine("\n6. feladat");
        string valasztottOsveny = osvenyek[osvenySorszam - 1];
        using (StreamWriter writer = new StreamWriter("kulonleges.txt"))
        {
            for (int i = 0; i < valasztottOsveny.Length; i++)
            {
                if (valasztottOsveny[i] == 'E' || valasztottOsveny[i] == 'V')
                {
                    writer.WriteLine($"{i + 1}\t{valasztottOsveny[i]}");
                }
            }
        }

        int[] poziciok = new int[jatekosokSzama];
        int maxTav = 0;
        int legtavolabbiJatekos = -1;
        int korekSzama = 0;

        for (int i = 0; i < dobasok.Length; i++)
        {
            int jatekos = i % jatekosokSzama;
            poziciok[jatekos] += dobasok[i];
            if (poziciok[jatekos] > maxTav)
            {
                maxTav = poziciok[jatekos];
                legtavolabbiJatekos = jatekos;
                korekSzama = i / jatekosokSzama + 1;
            }
        }

        Console.WriteLine("\n7. feladat");
        Console.WriteLine($"A játék a(z) {korekSzama}. körben fejeződött be. A legtávolabb jutó(k) sorszáma: {legtavolabbiJatekos + 1}");


        bool[] nyertesek = new bool[jatekosokSzama];
        int utolsoTeljesKor = dobasok.Length / jatekosokSzama;

        for (int i = 0; i < jatekosokSzama; i++)
        {
            poziciok[i] = 0;
        }

        for (int i = 0; i < dobasok.Length; i++)
        {
            int jatekos = i % jatekosokSzama;
            int dobas = dobasok[i];
            int ujPozicio = poziciok[jatekos] + dobas;


            if (ujPozicio < valasztottOsveny.Length)
            {
                if (valasztottOsveny[ujPozicio - 1] == 'E')
                {
                    ujPozicio += dobas;
                }
                else if (valasztottOsveny[ujPozicio - 1] == 'V')
                {
                    ujPozicio -= dobas;
                }
            }

            poziciok[jatekos] = Math.Min(ujPozicio, valasztottOsveny.Length);

            if (poziciok[jatekos] >= valasztottOsveny.Length && i / jatekosokSzama + 1 == utolsoTeljesKor)
            {
                nyertesek[jatekos] = true;
            }
        }

    }
}
