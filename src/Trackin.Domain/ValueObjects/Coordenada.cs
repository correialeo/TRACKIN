using System;

namespace Trackin.Domain.ValueObjects
{
    public class Coordenada : IEquatable<Coordenada>
    {
        public double X { get; }
        public double Y { get; }

        public Coordenada(double x, double y)
        {
            if (double.IsNaN(x) || double.IsInfinity(x))
                throw new ArgumentException("Coordenada X deve ser um valor numérico válido", nameof(x));

            if (double.IsNaN(y) || double.IsInfinity(y))
                throw new ArgumentException("Coordenada Y deve ser um valor numérico válido", nameof(y));

            X = x;
            Y = y;
        }

        public double DistanciaEuclidiana(Coordenada outraCoordenada)
        {
            if (outraCoordenada == null)
                throw new ArgumentNullException(nameof(outraCoordenada));

            double deltaX = X - outraCoordenada.X;
            double deltaY = Y - outraCoordenada.Y;

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public double DistanciaManhattan(Coordenada outraCoordenada)
        {
            if (outraCoordenada == null)
                throw new ArgumentNullException(nameof(outraCoordenada));

            return Math.Abs(X - outraCoordenada.X) + Math.Abs(Y - outraCoordenada.Y);
        }

        public bool EstaDentroDoRetangulo(Coordenada pontoInicial, Coordenada pontoFinal)
        {
            if (pontoInicial == null || pontoFinal == null)
                return false;

            double minX = Math.Min(pontoInicial.X, pontoFinal.X);
            double maxX = Math.Max(pontoInicial.X, pontoFinal.X);
            double minY = Math.Min(pontoInicial.Y, pontoFinal.Y);
            double maxY = Math.Max(pontoInicial.Y, pontoFinal.Y);

            return X >= minX && X <= maxX && Y >= minY && Y <= maxY;
        }

        public Coordenada Mover(double deltaX, double deltaY)
        {
            return new Coordenada(X + deltaX, Y + deltaY);
        }

        public string FormatarCoordenada(int precisao = 2)
        {
            return $"({X.ToString($"F{precisao}")}, {Y.ToString($"F{precisao}")})";
        }

        public bool Equals(Coordenada other)
        {
            if (other == null) return false;
            return Math.Abs(X - other.X) < 0.0001 && Math.Abs(Y - other.Y) < 0.0001;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordenada);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Math.Round(X, 4), Math.Round(Y, 4));
        }

        public static bool operator ==(Coordenada left, Coordenada right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Coordenada left, Coordenada right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return FormatarCoordenada();
        }

        public static Coordenada Origem() => new Coordenada(0, 0);

        public static Coordenada CriarValidando(double x, double y, double limiteX, double limiteY)
        {
            if (x < 0 || x > limiteX)
                throw new ArgumentException($"Coordenada X deve estar entre 0 e {limiteX}", nameof(x));

            if (y < 0 || y > limiteY)
                throw new ArgumentException($"Coordenada Y deve estar entre 0 e {limiteY}", nameof(y));

            return new Coordenada(x, y);
        }
    }
}