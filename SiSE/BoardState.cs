namespace SiSE;

public struct BoardState // TODO Doens't work couse the same objects for some reason don't work ;-;
{
    public int[,] Tiles { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        BoardState other = (BoardState)obj;

        if (Width != other.Width || Height != other.Height)
        {
            return false;
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Tiles[i, j] != other.Tiles[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }


    public bool Equals(BoardState other)
    {
        if (Width != other.Width || Height != other.Height)
        {
            return false;
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Tiles[i, j] != other.Tiles[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Width.GetHashCode();
        hash = hash * 23 + Height.GetHashCode();

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                hash = hash * 23 + Tiles[i, j].GetHashCode();
            }
        }

        return hash;
    }
}