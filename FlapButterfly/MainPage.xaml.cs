namespace FlapButterfly;

public partial class MainPage : ContentPage
{
    const int Gravidade = 30;
    const int TempoEntreFrames = 100;
    bool EstaMorto = true;
    double LarguraJanela = 0;
    double AlturaJanela = 0;
    double AlturaCano = 300;
    double AlturaJanelaInicial = 0;
    int Velocidade = 20;
    public MainPage()
    {
	InitializeComponent();
    }
    
    void AplicaGravidade()
    {
        imgBorboleta.TranslationY += Gravidade;
    } 
    async Task Desenhar()
    {
        while (!EstaMorto)
        {
            AplicaGravidade();
            await Task.Delay(TempoEntreFrames);
            GerenciaCanos();
        }
    } 
    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
        LarguraJanela = w;
        AlturaJanela = h;
        if (AlturaJanelaInicial > 0)
        {
            imgCanoCima.HeightRequest = AlturaCano * h / AlturaJanelaInicial;
            imgCanoBaixo.HeightRequest = AlturaCano * h / AlturaJanelaInicial;
        }
        else 
            AlturaJanelaInicial = h;
    }

    void GerenciaCanos()
    {
        imgCanoCima.TranslationX -= Velocidade;
        imgCanoBaixo.TranslationX -= Velocidade;
        if (imgCanoBaixo.TranslationX <= -LarguraJanela)
        {
            imgCanoBaixo.TranslationX = 0;
            imgCanoCima.TranslationX = 0;
        }
    }
    void GameOverClicked (object s, TappedEventArgs a)
    {
        frameGameOver.IsVisible = false;
        Inicializar();
        Desenhar();
    }
    void Inicializar()
    {
        EstaMorto = false;
        imgBorboleta.TranslationY = 0;
    }
}

