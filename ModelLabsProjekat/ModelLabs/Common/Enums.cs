using System;

namespace FTN.Common
{
    public enum PhaseCode : short
    {
        s1 = 0x0,
        s1N = 0x1,
        s2N = 0x2,
        ABC = 0x3,
        A = 0x4,
        BC = 0x5,
        AC = 0x6,
        B = 0x7,
        AB = 0x8,
        s2 = 0x9,
        ABN = 0xA,
        ACN = 0xB,
        C = 0xC,
        s12 = 0xD,
        s12N = 0xE,
        N = 0xF,
        BCN = 0x10,
        ABCN = 0x11,
        BN = 0x12,
        AN = 0x13,
        CN = 0x14,
        

    }

    public enum RegulatingControlModeKind : short
    {
        reactivePower = 0x0,
        powerFactor = 0x1,
        activePower = 0x2,
        temperature = 0x3,
        voltage = 0x4,
        @fixed = 0x5,
        timeScheduled = 0x6,
        admittance = 0x7,
        currentFlow = 0x8,
    }

    

    public enum UnitMultiplier : short
    {
        none = 0x0,
        m = 0x1,
        G = 0x2,
        n = 0x3,
        d = 0x4,
        k = 0x5,
        c = 0x6,
        T = 0x7,
        M = 0x8,
        micro = 0x9,
        p = 0xA,
    }

    public enum UnitSymbol : short
    {
        A = 0x0,
        J = 0x1,
        Hz = 0x2,
        ohm = 0x3,
        deg = 0x4,
        Wh = 0x5,
        S = 0x6,
        VAr = 0x7,
        m = 0x8,
        VA = 0x9,
        VAh = 0xA,
        H = 0xB,
        N = 0xC,
        Pa = 0xD,
        h = 0xE,
        V = 0xF,
        g = 0x10,
        none = 0x11,
        W = 0x12,
        rad = 0x13,
        VArh = 0x14,
        m3 = 0x15,
        degC = 0x16,
        F = 0x17,
        s = 0x18,
        min = 0x19,
        m2 = 0x1A,
    }
}
