 //TR-11  Natalie Bratchykova
  
 using System;
 class Program
{
    public static void Main(string[] args){

        GameAccount gameAccount = new GameAccount("Natalie");
        GameAccount gameAccount1 = new GameAccount("Kirill");
        GameAccount gameAccount2 = new GameAccount("Hayao");
        GameAccount gameAccount3 = new GameAccount("Mikael");


        gameAccount.playGame(gameAccount1);
        gameAccount1.playGame(gameAccount);
        gameAccount1.playGame(gameAccount2);
        gameAccount.playGame(gameAccount2);
        gameAccount.playGame(gameAccount3);
        gameAccount.playGame(gameAccount3);
        gameAccount.playGame(gameAccount3);
        gameAccount.playGame(gameAccount2);

        gameAccount1.playGame(gameAccount2);
        gameAccount1.playGame(gameAccount3);
        gameAccount1.playGame(gameAccount2);
        gameAccount1.playGame(gameAccount);

        gameAccount2.playGame(gameAccount);
        gameAccount2.playGame(gameAccount1);
        gameAccount2.playGame(gameAccount3);

        Console.WriteLine(gameAccount.GetStatus());
        Console.WriteLine(gameAccount1.GetStatus());
        Console.WriteLine(gameAccount2.GetStatus());
        Console.WriteLine(gameAccount3.GetStatus());

    }
}

enum GameResultStatus{win, loose};

class GameAccount{

    public string UserName{get; set;}
    public int CurrentRating{get; set;}
    private List<History> gameAccountStatus = new List<History>();

    public GameAccount(string name){
        UserName = name;
        CurrentRating = new Random().Next(1,20);
    }

    public void playGame(GameAccount opponent){

        Game game  = new Game(this, opponent);
        int dice1 = new Random().Next(1,6);
        
        if (dice1 >= 3)
        {
            WinGame(opponent, game);
        }
        else
        {
            LooseGame(opponent, game);
        }
    }


    public void WinGame(GameAccount opponent,  Game game){
       // check if opponent's rating is more than 0 (if it's less output exeption)
       GetException(CheckRating(opponent, game.GameRating), opponent);

    // check his rating again and if it's less than 0 it's align new value for it = 1
    // else it substracts game's rating
        opponent.CurrentRating = (CheckRating(opponent, game.GameRating)) ? opponent.CurrentRating - game.GameRating : 1;
    // add game rating to winner player's rating  
        this.CurrentRating += game.GameRating;
    // call PlayedGameResult method
        PlayedGameResult(this, opponent, game);
    }

    public void LooseGame(GameAccount opponent,  Game game){
        opponent.CurrentRating += game.GameRating;
        GetException(CheckRating(this, game.GameRating), this);
        this.CurrentRating = (CheckRating(this, game.GameRating)) ? this.CurrentRating - game.GameRating : 1;
        PlayedGameResult(opponent, this,  game);

    }

// if after substraction player's rating become less than 0 returns false, else returns true
    public Boolean CheckRating(GameAccount user, int gameRating){
        return (user.CurrentRating - gameRating < 1) ? false: true;
    }

// if player's rating is less than 0 warning about error
    public void GetException(Boolean checkRating, GameAccount player){
        if(!checkRating){
            Console.WriteLine($"ERROR! {player.UserName}'S RATING IS LESS THAN 0\nUPDATED {player.UserName}'S RATING = 1\n");
        }
    }


    // add information about game results to list
    public void PlayedGameResult(GameAccount user, GameAccount opponent, Game game){
     
        user.gameAccountStatus.Add(new History(user, opponent, game, GameResultStatus.win));
        opponent.gameAccountStatus.Add(new History(opponent, user, game, GameResultStatus.loose));
    }

    public string GetStatus(){
        var result = new System.Text.StringBuilder();
        result.AppendLine($"for current user - {this.UserName}\n");
        
        result.AppendLine("Game's ID\t\tPoints\tStatus\tRating\t\tOpponent\tOpponent's points\tOpponent's Status");
        foreach(var item in gameAccountStatus){
            result.AppendLine($"{item.GameIdStr}\t\t{item.UserGameResultRating}\t{item.UserGameStatus}\t{item.MainUserRating}\t\t{item.OpponentUser.UserName}\t\t{item.OpponentGameResultRating}\t\t\t{item.OpponentGameStatus}");
        }

        return result.ToString();
    }
}


class Game{
    private static int GameId = 20042904;
    public string GameIdStr;
    public int GameRating{get; set;}

    public Game(GameAccount user, GameAccount opponentUser){
        GameIdStr = GameId.ToString();
        GameId++;        
        GameRating = new Random().Next(1, 15);    
    }

}

class History{
    
    // for save Game's ID  for outputting in game's history
    public string GameIdStr;

    public GameAccount MainUser;
    public GameAccount OpponentUser;

    public string UserGameResultRating;

    public string UserGameStatus{set; get;}
    public string OpponentGameStatus{set; get;}
    
    public int MainUserRating;
       
    public string OpponentGameResultRating;
    

        public History(GameAccount user, GameAccount opponentUser, Game game, GameResultStatus status){
    
        this.GameIdStr = game.GameIdStr;
        MainUser = user;
        this.OpponentUser = opponentUser;

        MainUserRating = MainUser.CurrentRating;
        
       UserGameStatus = status.ToString();
       
       // check if user game's status == win
       if(UserGameStatus.Equals("win")){
        // if user's status = win, opponent's should be loose
        OpponentGameStatus = GameResultStatus.loose.ToString();
        // user won -> he got poitive result (add game's rating to his own)
        UserGameResultRating = game.GameRating.ToString();
        // opponent lost -> he got negative result (substract game's rating from his own)
        OpponentGameResultRating = (game.GameRating * -1).ToString();
       }

       else{
        OpponentGameStatus = GameResultStatus.win.ToString();
        OpponentGameResultRating = game.GameRating.ToString();
        UserGameResultRating = (game.GameRating * -1).ToString();
       }
        }
}
