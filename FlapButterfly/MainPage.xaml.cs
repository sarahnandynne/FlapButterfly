namespace FlapButterfly;

public partial class MainPage : ContentPage
{
    const int Gravidade = 50;
    const int TempoEntreFrames = 1000;
    bool EstaMorto = false;
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
        }
    } 
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Desenhar();
    }
}

