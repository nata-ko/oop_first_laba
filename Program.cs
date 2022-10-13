 
 using System;
 class Program
{
    public static void Main(string[] args){

        GameAccount gameAccount = new GameAccount("Natalie");
        GameAccount gameAccount1 = new GameAccount("Kirill");
        GameAccount gameAccount2 = new GameAccount("Hayao");
        GameAccount gameAccount3 = new GameAccount("Mikael");

        gameAccount.playGame(gameAccount1);
        gameAccount.playGame(gameAccount2);
        gameAccount.playGame(gameAccount3);
        gameAccount.playGame(gameAccount2);
        gameAccount.playGame(gameAccount1);
        gameAccount.playGame(gameAccount1);
        gameAccount.playGame(gameAccount2);
        gameAccount.playGame(gameAccount2);
        gameAccount.playGame(gameAccount3);

        gameAccount1.playGame(gameAccount2);
        gameAccount1.playGame(gameAccount3);
        gameAccount1.playGame(gameAccount2);
        gameAccount1.playGame(gameAccount);
        gameAccount1.playGame(gameAccount);
        gameAccount1.playGame(gameAccount3);
        gameAccount1.playGame(gameAccount3);

        gameAccount2.playGame(gameAccount);
        gameAccount2.playGame(gameAccount1);
        gameAccount2.playGame(gameAccount3);
        gameAccount2.playGame(gameAccount1);
        gameAccount2.playGame(gameAccount);
        gameAccount2.playGame(gameAccount1);
        gameAccount2.playGame(gameAccount3);


        Console.WriteLine(gameAccount.getStatus());
        Console.WriteLine(gameAccount1.getStatus());
        Console.WriteLine(gameAccount2.getStatus());
        Console.WriteLine(gameAccount3.getStatus());

    }
}


class GameAccount{

    public string UserName{get; set;}
    public int CurrentRating{get; set;}
    public string GameResultRating = "";
    public int GamesCount;
    public string OpponentName = "";
    private List<Game> gameAccountStatus = new List<Game>();
    public string GameResult = "";


    public GameAccount(string name){
        UserName = name;
        GamesCount = 0;
        CurrentRating = new Random().Next(1,100);
    }

    public void playGame(GameAccount opponent){

        Game game  = new Game(this, opponent);

        int dice1 = new Random().Next(1,6);
        int dice2 = new Random().Next(1,6);

        if(dice1 > dice2){
            WinGame(opponent, game.GameRating);
        }
        else if(dice1 < dice2){
            LooseGame(opponent, game.GameRating);
        }
        else{
            dice1 = new Random().Next(1,6);
        }
    }


    public void WinGame(GameAccount opponent, int GameRating){
        this.OpponentName = opponent.UserName;//user's opponent -> opponent
        opponent.OpponentName = this.UserName;// opponent's opponent -> user
        
        this.GameResult = "win"; //user -> win
        opponent.GameResult = "loose"; // opponent -> loose

        this.GameResultRating = "+" + GameRating.ToString();//user +
        opponent.GameResultRating = "-" + GameRating.ToString();//opponent -


        opponent.CurrentRating = (checkRating(opponent, GameRating)) ? 1: opponent.CurrentRating - GameRating;

        this.CurrentRating += GameRating;

        Game winGame = new Game(this, opponent);
        Game opWinGame = new Game(opponent, this);
        opWinGame.GameIdStr = winGame.GameIdStr;

        gameAccountStatus.Add(winGame); 
        opponent.gameAccountStatus.Add(opWinGame);

    }

    public void LooseGame(GameAccount opponent, int GameRating){

        this.OpponentName = opponent.UserName;
        opponent.OpponentName = this.UserName;

        opponent.CurrentRating += GameRating;

        opponent.GameResult = "win";
        this.GameResult = "loose";


        this.GameResultRating = "-" + GameRating.ToString();
        opponent.GameResultRating = "+" + GameRating.ToString();


        this.CurrentRating = (checkRating(this, GameRating)) ? this.CurrentRating - GameRating : 1;

        Game loose = new Game(this, opponent);
        Game opLoose = new Game(opponent, this);
        
        opLoose.GameIdStr = loose.GameIdStr;
        gameAccountStatus.Add(loose);
        opponent.gameAccountStatus.Add(opLoose);
    }

    public Boolean checkRating(GameAccount user, int GameRating){

        Boolean isChecked = false;
        return isChecked = (user.CurrentRating < 0) ? throw new ArgumentOutOfRangeException(nameof(user.CurrentRating), $"{user.UserName}'s rating is less than 0"): isChecked = true;
    }

    public string getStatus(){
        var result = new System.Text.StringBuilder();
        result.AppendLine($"for current user - {this.UserName}\n");
        
        result.AppendLine("Game's ID\t\tPoints\tStatus\tRating\t\tOpponent\tOpponent's points\tOpponent's Status");
        foreach(var item in gameAccountStatus){
            result.AppendLine($"{item.GameIdStr}\t\t{item.GameResultRating}\t{item.GameResult}\t{item.mainUserRating.ToString()}\t\t{item.OpponentName}\t\t{item.OpponentGameResultRating}\t\t\t{item.OpponentGameResult}");
        }

        return result.ToString();
    }
}


class Game{
    public static int GameId = 20042904;
    public string GameIdStr;
    public int GameRating{get; set;}
    public int mainUserRating;
    public string GameResultRating;
    public string GameRatingStr{get; set;}
    public string GameResult{set; get;}
    public string OpponentName{get; set;}
    public int opponentUserRating;
    public string OpponentGameResult{set; get;}
    public string OpponentGameResultRating;
    GameAccount mainUser;
    GameAccount OpponentUser;


    public Game(GameAccount user, GameAccount opponentUser){
        GameIdStr = GameId.ToString();
        GameId++;

        mainUser = user;
        this.OpponentUser = opponentUser;

        mainUserRating = mainUser.CurrentRating;
        opponentUserRating = OpponentUser.CurrentRating;

        OpponentName = this.OpponentUser.UserName;
        GameResult = mainUser.GameResult;
        GameRatingStr = GameRating.ToString();

        GameRating = new Random().Next(1, 15);
        OpponentGameResult = opponentUser.GameResult;//save not opponent result

        GameResultRating = mainUser.GameResultRating;
        OpponentGameResultRating = this.OpponentUser.GameResultRating;
    }

}