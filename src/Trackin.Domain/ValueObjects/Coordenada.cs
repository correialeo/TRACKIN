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

        public static Coordenada Centro(Coordenada ponto1, Coordenada ponto2)
        {
            if (ponto1 == null || ponto2 == null)
                throw new ArgumentNullException("Pontos não podem ser nulos");

            return new Coordenada(
                (ponto1.X + ponto2.X) / 2,
                (ponto1.Y + ponto2.Y) / 2
            );
        }

        public bool EstaDentroDoCirculo(Coordenada centro, double raio)
        {
            if (centro == null)
                return false;

            return DistanciaEuclidiana(centro) <= raio;
        }

        public double AnguloPara(Coordenada destino)
        {
            if (destino == null)
                throw new ArgumentNullException(nameof(destino));

            double deltaX = destino.X - X;
            double deltaY = destino.Y - Y;

            return Math.Atan2(deltaY, deltaX) * 180 / Math.PI;
        }

        public Coordenada Rotacionar(Coordenada centro, double anguloGraus)
        {
            if (centro == null)
                throw new ArgumentNullException(nameof(centro));

            double anguloRadianos = anguloGraus * Math.PI / 180;
            double cos = Math.Cos(anguloRadianos);
            double sin = Math.Sin(anguloRadianos);

            double deltaX = X - centro.X;
            double deltaY = Y - centro.Y;

            double novoX = deltaX * cos - deltaY * sin + centro.X;
            double novoY = deltaX * sin + deltaY * cos + centro.Y;

            return new Coordenada(novoX, novoY);
        }

        public bool EstaNaLinha(Coordenada ponto1, Coordenada ponto2, double tolerancia = 0.1)
        {
            if (ponto1 == null || ponto2 == null)
                return false;

            double distancia = Math.Abs((ponto2.Y - ponto1.Y) * X - (ponto2.X - ponto1.X) * Y + 
                                      ponto2.X * ponto1.Y - ponto2.Y * ponto1.X) / 
                              Math.Sqrt(Math.Pow(ponto2.Y - ponto1.Y, 2) + Math.Pow(ponto2.X - ponto1.X, 2));

            return distancia <= tolerancia;
        }
    }
}