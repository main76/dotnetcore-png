namespace Masteryu.Png
{
    /// <summary>
    /// All Types of PNG's Chunk
    /// </summary>
    public enum ChunkType
    {
        // Each enum value is precalculated, for example:
        // IHDR = ((int)'I' << 24) + ((int)'H' << 16) + ((int)'D' << 8) + (int)'R';
        // Except NULL, of course.
        
        #region Critical chunks

        IHDR = 1229472850,
        PLTE = 1347179589,
        IDAT = 1229209940,
        IEND = 1229278788,

        #endregion

        #region Ancillary chunks

        cHRM = 1665684045,
        gAMA = 1732332865,
        iCCP = 1766015824,
        sBIT = 1933723988,
        sRGB = 1934772034,
        bKGD = 1649100612,
        hIST = 1749635924,
        tRNS = 1951551059,
        pHYs = 1883789683,
        sPLT = 1934642260,
        tIME = 1950960965,
        iTXt = 1767135348,
        tEXt = 1950701684,
        zTXt = 2052348020,

        #endregion

        NULL = 0
    }
}