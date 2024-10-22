﻿namespace FlapButterfly;

public partial class MainPage : ContentPage
{
    const int Gravidade = 8;
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
    int Velocidade = 10;
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
			var posHBorboleta = (LarguraJanela / 2) - (imgBorboleta.WidthRequest / 2);
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
			var posHBorboleta = (LarguraJanela / 2) - (imgBorboleta.WidthRequest / 2);
			var posVBorboleta = (AlturaJanela / 2) - (imgBorboleta.HeightRequest / 2) + imgBorboleta.TranslationY;
			if (posHBorboleta >= Math.Abs(imgCanoBaixo.TranslationX) - imgCanoBaixo.WidthRequest &&
			posHBorboleta <= Math.Abs(imgCanoBaixo.TranslationX) + imgCanoBaixo.WidthRequest &&
			posVBorboleta <= imgCanoBaixo.HeightRequest + imgCanoBaixo.TranslationY)

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
    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
        LarguraJanela = w;
        AlturaJanela = h;
    }

    void GerenciaCanos()
    {
        imgCanoCima.TranslationX -= Velocidade;
        imgCanoBaixo.TranslationX -= Velocidade;
        if (imgCanoBaixo.TranslationX <= -LarguraJanela)
        {
            imgCanoBaixo.TranslationX = 0;
            imgCanoCima.TranslationX = 0;
            var AlturaMax = -05;
            var AlturaMin = -imgCanoBaixo.HeightRequest;
            imgCanoCima.TranslationY = Random.Shared.Next((int)AlturaMin, (int)AlturaMax);
            imgCanoBaixo.TranslationY = imgCanoCima.TranslationY + AberturaMinima;
            Score++;
            labelScore.Text = "Score: " + Score.ToString("D3");
            labelFrase.Text = "VOCÊ PASSOU POR: " + Score.ToString("D3") + " CANOS";
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
        imgCanoCima.TranslationX = -LarguraJanela;
        imgCanoBaixo.TranslationX = -LarguraJanela;
        imgBorboleta.TranslationX = 0;
        imgBorboleta.TranslationY = 0;
        Score = 0;
        GerenciaCanos();
    }
    void OnGridClicked (object sender, TappedEventArgs a) 
    {
        EstaPulando = true;
    }
}

