using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_KASSE
{
    public enum GVTypEnum
    {
        Umsatz = 1,
        Pfand = 2,
        PfandRueckzahlung = 3,
        Rabatt = 4,
        Aufschlag = 5,
        ZuschussEcht = 6,
        ZuschussUnecht = 7,
        TrinkgeldAG = 8,
        TrinkgeldAN = 9,
        EinzweckgutscheinKauf = 10,
        EinzweckgutscheinEinloesung = 11,
        MehrzweckgutscheinKauf = 12,
        MehrzweckgutscheinEinloesung = 13,
        Forderungsentstehung = 14,
        Forderungsaufloesung = 15,
        Anzahlungseinstellung = 16,
        Anzahlungsaufloesung = 17,
        Anfangsbestand = 18,
        Privatentnahme = 19,
        Privateinlage = 20,
        Geldtransit = 21,
        Lohnzahlung = 22,
        Einzahlung = 23,
        Auszahlung=24,
        DifferenzSollIst=25
    }
}
