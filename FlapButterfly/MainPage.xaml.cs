namespace FlapButterfly;

public partial class MainPage : ContentPage
{
    const int Gravidade = 5;
    const int TempoEntreFrames = 25;
    bool EstaMorto = true;
    double LarguraJanela = 0;
    double AlturaJanela = 0;
    double AlturaCano = 300;
    const int MaxTempoPulando = 3;
    int Score = 0;
    const int ForcaPulo = 20;
    int TempoPulando = 0;
    const int AberturaMinima = 200;
    bool EstaPulando = false;
    int Velocidade = 5;
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
            if (EstaPulando)
                AplicaPulo();
            else
                AplicaGravidade();
            GerenciaCanos();
            if (VerificaColisao())
            {
                EstaMorto = true;
                SoundHelper.Play("gameover.wav");
                frameGameOver.IsVisible = true;
                break;
            }
            await Task.Delay(TempoEntreFrames);
        }
    }
    bool VerificaColisao()
    {
        if (!EstaMorto)
        {
            if (VerificaColisaoTeto() ||
            VerificaColisaoChao() ||
            VerificaColisaoCanoCima() ||
            VerificaColisaoCanoBaixo())
            {
                return true;
            }
        }
        return false;
    }

    bool VerificaColisaoCanoCima()
    {
        var posHBorboleta = (LarguraJanela - 50) - (imgBorboleta.WidthRequest / 2);
        var posVBorboleta = (AlturaJanela / 2) - (imgBorboleta.HeightRequest / 2) + imgBorboleta.TranslationY;
        if (posHBorboleta >= Math.Abs(imgCanoCima.TranslationX) - imgCanoCima.WidthRequest &&
        posHBorboleta <= Math.Abs(imgCanoCima.TranslationX) + imgCanoCima.WidthRequest &&
        posVBorboleta <= imgCanoCima.HeightRequest + imgCanoCima.TranslationY)

        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool VerificaColisaoCanoBaixo()
    {
        var posHBorboleta = (LarguraJanela - 50) - (imgBorboleta.WidthRequest / 2);
        var posVBorboleta = (AlturaJanela / 2) + (imgBorboleta.HeightRequest / 2) + imgBorboleta.TranslationY;
        var yMaxCano = imgCanoCima.HeightRequest + imgCanoCima.TranslationY + AberturaMinima;
        if (posHBorboleta >= Math.Abs(imgCanoBaixo.TranslationX) - imgCanoBaixo.WidthRequest &&
           posHBorboleta <= Math.Abs(imgCanoBaixo.TranslationX) + imgCanoBaixo.WidthRequest &&
           posVBorboleta >= yMaxCano)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool VerificaColisaoTeto()
    {
        var minY = -AlturaJanela / 2;
        if (imgBorboleta.TranslationY <= minY)
            return true;
        else
            return false;

    }
    bool VerificaColisaoChao()
    {
        var maxY = AlturaJanela / 2 - imgChao.HeightRequest;
        if (imgBorboleta.TranslationY >= maxY)
            return true;
        else
            return false;
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        LarguraJanela = width;
        AlturaJanela = height;
        if (Height > 0)
        {
            imgCanoCima.HeightRequest = AlturaJanela;
            imgCanoBaixo.HeightRequest = AlturaJanela;
        }
    }

    void GerenciaCanos()
    {
        imgCanoCima.TranslationX -= Velocidade;
        imgCanoBaixo.TranslationX -= Velocidade;
        if (imgCanoBaixo.TranslationX <= -LarguraJanela)
        {
            imgCanoBaixo.TranslationX = 0;
            imgCanoCima.TranslationX = 0;
            var AlturaMaxima = -(imgCanoBaixo.HeightRequest * 0.2);
            var AlturaMinima = -imgCanoBaixo.HeightRequest * 0.8;
            imgCanoCima.TranslationY = Random.Shared.Next((int)AlturaMinima, (int)AlturaMaxima);
            imgCanoBaixo.TranslationY = imgCanoCima.HeightRequest + imgCanoCima.TranslationY + AberturaMinima;
            Score++;
            labelScore.Text = "Score: " + Score.ToString("D3");
             SoundHelper.Play("pontuação.wav");
            labelFrase.Text = "VOCÊ PASSOU POR: " + Score.ToString("D3") + " CANOS";
            if (Score % 2 == 0)
                Velocidade++;
        }
    }
    void AplicaPulo()
    {
        imgBorboleta.TranslationY -= ForcaPulo;
        TempoPulando++;
        if (TempoPulando >= MaxTempoPulando)
        {
            EstaPulando = false;
            TempoPulando = 0;
            SoundHelper.Play("começo.wav");
        }
    }
    void GameOverClicked(object s, TappedEventArgs a)
    {
        frameGameOver.IsVisible = false;
        Inicializar();
        Desenhar();
    }
    void Inicializar()
    {
        EstaMorto = false;
        SoundHelper.Play("começo.wav");
        imgCanoCima.TranslationX = -LarguraJanela;
        imgCanoBaixo.TranslationX = -LarguraJanela;
        imgBorboleta.TranslationX = 0;
        imgBorboleta.TranslationY = 0;
        Score = 0;
        Velocidade = 5;
        GerenciaCanos();
    }
    void OnGridClicked(object sender, TappedEventArgs a)
    {
        EstaPulando = true;
    }
}

