namespace examen_marlon_de_la_cruz
{
    public class Alquiler
    {
        public string Vehiculo { get; set; }
        public decimal Tarifa { get; set; }
        public int Dias { get; set; }
        public bool SeguroCompleto { get; set; }
        public bool GPS { get; set; }

        public decimal CalcularTotal()
        {
            decimal total = Tarifa * Dias;
            if (SeguroCompleto)
                total += 300m * Dias;
            if (GPS)
                total += 100m * Dias;
            return total;
        }
    }
}
