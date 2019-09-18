namespace Sprinters.SharedTypes.Shared
{
    public static class NameConstants
    {

        public const decimal BtwTarief = 1.21m;

        //Bestelling service
        public const string NieuweBestellingCommandQueue = "Sprinters.BestellingBeheer.NieuweBestelling";
        public const string VolgendeBestellingCommandQueue = "Sprinters.BestellingBeheer.VolgendeBestelling";
        public const string FinishBestellingCommandQueue = "Sprinters.BestellingBeheer.VoltooiBestelling";
        public const string RegistreerGebruikerCommandQueue = "Sprinters.Authenticatie.RegistreerGebruiker";
        public const string LogGebuikerInCommandQueue = "Sprinters.Authenticatie.LogGebruikerIn";
        public const string VerwijderGebruikerCommandQueue = "Sprinters.Authenticatie.VerwijderGebruiker";
        public const string BestellingGoedKeurenCommandQueue = "Sprinters.BestellingBeheer.KeurBestellingGoed";
        public const string BestellingAfkeurenCommandQueue = "Sprinters.BestellingBeheer.KeurBestellingAf";

        public const string BestellingToegevoegdEvent = "Sprinters.BestellingBeheer.BestellingToegevoegd";
        public const string BestellingVerzondenEvent = "Sprinters.BestellingBeheer.BestellingVerzonden";
        public const string BestellingInpakkenGestartEvent = "Sprinters.BestellingBeheer.BestellingInpakkenGestart";
        public const string BestellingGoedgekeurdEvent = "Sprinters.BestellingBeheer.BestellingGoedgekeurdEvent";
        public const string BestellingAfgekeurdEvent = "Sprinters.BestellingBeheer.BestellingAfgekeurdEvent";

        //Klant service
        public const string RegistreerKlantCommandQueue = "Sprinters.KlantBeheer.RegistreerKlant";

        public const string KlantGeregistreerdEvent = "Sprinters.KlantBeheer.KlantGeregistreerdEvent";


        //KlantService
        public const decimal KredietLimit = 500m;
        public const string KlantKredietAangepastEvent = "Sprinters.BestellingService.KlantKredietAangepastEvent";

        public const string BetaalBestellingCommandQueue = "Sprinters.BestellingService.BetaalBestellingQueue";

    }
}