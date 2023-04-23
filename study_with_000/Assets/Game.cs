/*
	
	隠しホラー
	〇〇〇とおべんきょう（殺人鬼とおべんきょう）

	Aspect：　iPhone X/XS 2436x1125 Portrait

	Produced by Maya Akahane
	2021/06/30

*/

using GameCanvas;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;


public sealed class Game : GameBase
{
  // 変数の宣言        
	int window_w = 640;
	int window_h = 1000;
	// int window_w = 640;
	// int window_h = 480;

	int gameState = 0; // titile: 0 , GameOver: -1, GamePlaying: 1, clear: 2
	int stage = 0;
	string doorFlag = "0";
	string correctFlag;
	int horrorPt = 0;
	int horrorFlag = 0;
	int phase = 0;		// correctFlag=="Open"時のアニメーションに使用
	int step = 0;


  int player_x = 220;
	int player_y = 800;
	double pointer_x;
	double pointer_y;
	
	GcImage player_img = GcImage.N_bear; // normal_bear(120x120)  hr_bear(200x200)
	GcImage wDoor_closed = GcImage.N_wDoor_closed; // doors(150x258?)
	GcImage wwDoor_opened = GcImage.N_wwDoor_opened;
	GcImage murder_front = GcImage.Hr_murderer_front; // murderer(120x250)
	GcImage murder_back = GcImage.Hr_murderer_back; 
	GcImage murder_side = GcImage.Hr_murderer_side1; // around (130 x 250)
	GcImage gate_closed = GcImage.Hr_gate_closed; // gates(400x346) (murdererのheightは270がいい)

	int time = 0;
	int tmpTime;
	int count;
	int rism = 1;
	int slowRism = 1;

	int a_door_x = 50;	//50
	int b_door_x = 245;	//245
	int c_door_x = 440; //440
	int door_y = 300;
	int score = 0;

	int startBox_x = 170;
	int startBox_y = 600;
	int startBox_w = 300;
	int startBox_h = 100;
	int nextBox_x = 260;
	int nextBox_y = 800;
	int nextBox_w = 120;
	int nextBox_h = 60;

	string question;
	string anti_question;
	string answer_a;
	string answer_b;
	string answer_c;
	string answer;



	public override void InitGame()
	{
		gc.SetResolution(640, 1000);
	}

	public override void UpdateGame()
	{
		pointer_x = 0;
		pointer_y = 0;
		pointer_x = gc.GetPointerX(0);
		pointer_y = gc.GetPointerY(0);


		time += 1;
		// クマの画像上下のリズム
		if (time%20==0) rism *= -1;
		if (time%40==0) slowRism += 1;


		if(horrorFlag == 0) normalUpdate();
		if(horrorFlag == 1) {
			horrrorUpdate();
		}

	}


	public override void DrawGame()
	{
		gc.ClearScreen();
		// gc.DrawString("horrrorPt: "+horrorPt, 5, 30);

		if(horrorFlag == 0){
			normalDraw();
		}
		else if(horrorFlag == 1){
			horrorDraw();
		}
		

	}

	void restartGame(){
		correctFlag = "0";
		doorFlag = "0";
		stage = 0;
		pointer_x = 0;
		pointer_y = 0;
		horrorPt = 0;
		horrorFlag = 0;
	}
	// horrorFlag のリセットなし
	void resetValues(){
		correctFlag = "0";
		doorFlag = "0";
		stage = 0;
		pointer_x = 0;
		pointer_y = 0;
		horrorPt = 0;
		phase = 0;
		step = 0;
		gameState = 0;
	}

	void normalUpdate(){

		// Title
		if(gameState == 0){
			if((int)pointer_x > startBox_x && (int)pointer_x < startBox_x+startBox_w){
				if((int)pointer_y > startBox_y && (int)pointer_y < startBox_y+startBox_h){			
					stage += 1;
					tmpTime = time;
					gameState += 1;
				}
			}

		}
		// Game Clear
		else if(gameState == 2){
			if(gc.GetPointerFrameCount(0) > 30){
				gameState = 0;
				restartGame();
			}
		}
		// Game Playing
		else if(gameState == 1){

			if((time-tmpTime)>15 && (time-tmpTime)<30) phase = 1;
			else if((time-tmpTime)>300 && (time-tmpTime)<360) phase = 1;
			else if((time-tmpTime)>600 && (time-tmpTime)<800) phase = 1;
			else phase = 0;
				
			// door open flag (a:左 b:中央 c:右)
			if(door_y < (int)pointer_y && (int)pointer_y < door_y+258 && correctFlag != "True"){
				//door a
				if(a_door_x < (int)pointer_x && (int)pointer_x < a_door_x+150){
					doorFlag = "a";
					correctFlag = "False";
					gc.PlaySound(GcSound.Incorrect_short);
					if(stage == 2) {
						// horrorPt += 1; // second horror answer
						correctFlag = "Dark";
					}
					
				//door b
				}else if(b_door_x < (int)pointer_x && (int)pointer_x < b_door_x+150){
					doorFlag = "b";
					if(stage == 1) {
						score += 1;
						correctFlag = "True";
						gc.PlaySound(GcSound.Correct);
					} else {
						correctFlag = "False";
						gc.PlaySound(GcSound.Incorrect_short);

						if(stage == 3){		// last horror answer
							// horrorPt += 1;
							correctFlag = "Dark";
						}
					}
				//door c
				}else if(c_door_x < (int)pointer_x && (int)pointer_x < c_door_x+150){
					doorFlag = "c";
					if(stage == 2 || stage == 3) {
						// 正解
						score += 1;
						correctFlag = "True";
						gc.PlaySound(GcSound.Correct);
					} else {
						// 不正解
						correctFlag = "False";
						gc.PlaySound(GcSound.Incorrect_short);
						if(stage == 1){ // first horror answer
							// horrorPt += 1;	
							correctFlag = "Dark";
						}
					}
				}
			}

			// 正解画面
			if(correctFlag=="True" || correctFlag=="False"){
				if((int)pointer_x > nextBox_x && (int)pointer_x < nextBox_x+nextBox_w){
					if((int)pointer_y > nextBox_y && (int)pointer_y < nextBox_y+nextBox_h){
						//init
						correctFlag = "0";
						doorFlag = "0";
						tmpTime = time;
						stage += 1;
						pointer_x = 0;
						pointer_y = 0;
					}
				}

			// 逆符号不正解
			}else if(correctFlag == "Dark"){
				// if(gc.GetKeyPressFrameCount(KeyCode.Space) == 1){
				// if(gc.GetPointerFrameCount(0) == 1){
				if((int)pointer_x > nextBox_x && (int)pointer_x < nextBox_x+nextBox_w){
					if((int)pointer_y > nextBox_y && (int)pointer_y < nextBox_y+nextBox_h){
						correctFlag = "0";
						doorFlag = "0";
						stage += 1;
						pointer_x = 0;
						pointer_y = 0;
						horrorPt += 1;
						tmpTime = time;

						if(horrorPt == 3) {
							resetValues();
							tmpTime = time;
							horrorFlag = 1;  //turn on horrorFlag
						}

					}
				}
			}

			if(stage==4){
				gameState += 1; //game clear
			}
		}
	}

	void normalDraw(){
		gc.ClearScreen();
		gc.SetBackgroundColor(1f,0.92f,0.75f);

		// クマの画像上下（速度はrism参照）
		if(rism > 0) gc.DrawImage(player_img, player_x, player_y);
		else if (rism < 0) gc.DrawImage(player_img, player_x, player_y+5);

		// Title
		if(gameState == 0){
			gc.SetColor(100,100, 255);
			gc.SetFontSize(50);
			gc.DrawString("◯◯◯とおべんきょう", window_w/2-250, 120);
			gc.SetColor(255,0,0);
			gc.FillRect(startBox_x, startBox_y, startBox_w, startBox_h);
			gc.SetColor(255,255,255);
			gc.DrawString("START", window_w/2-65, startBox_y+30);
		}

		// Game Clear
		else if (gameState == 2){
			gc.SetColor(0,0,0);
			gc.SetFontSize(80);
			if(score < 3) gc.DrawString("Finished!", window_w/2-180, 300);
			else {
				gc.SetColor(255,0,0);
				gc.DrawString("FULL SCORE!", window_w/2-200, 300);
			}
			gc.SetFontSize(50);
			gc.SetColor(0,0,0);
			gc.DrawString("SCORE "+score+"/3", window_w/2-120, window_h/2-100);
			gc.SetFontSize(30);
			gc.DrawString("長押しでリプレイ", window_w/2-120, window_h/2+200);
			
		}

		// Game Playing
		else if (gameState == 1){
			//status
			gc.SetColor(0,0,0);

			// doors animation
			gc.DrawImage(wDoor_closed, a_door_x, door_y);
			gc.DrawImage(wDoor_closed, b_door_x, door_y);
			gc.DrawImage(wDoor_closed, c_door_x, door_y);


			// 問題内容
			if(stage == 1){
				question = "1 + 1 = ?";
				anti_question = "1 - 1 = ?";
				answer_a = "3";	
				answer_b = "2";	//correct
				answer_c = "0";
				answer = "1 + 1 = 2";
			} else if (stage == 2){
				question = "9 x 3 = ?";
				anti_question = "9 ÷ 3 = ?";
				answer_a = "3";
				answer_b = "93";
				answer_c = "27"; //correct
				answer = "9 x 3 = 27";
			} else if (stage == 3){
				question = "450 ÷ 30 = ?";
				anti_question = "450 x 30 = ?";
				answer_a = "5";
				answer_b = "13500";
				answer_c = "15"; //correct
				answer = "450 ÷ 30 = 15";
			} 

			// 問題描画
			gc.SetColor(0,0,0);
			if(phase == 0) gc.DrawString(question, window_w/2-100, 150); //question
			else if(phase == 1) {
				gc.SetColor(255,0,0);
				gc.DrawString(anti_question, window_w/2-100, 150); //逆符号
			}
			gc.SetColor(0,0,0);
			gc.DrawString(answer_a, a_door_x+10, door_y-50);	//a
			gc.DrawString(answer_b, b_door_x+10, door_y-50);	//b
			gc.DrawString(answer_c, c_door_x+10, door_y-50);	//c

			// 正解画面
			if(correctFlag == "True"){
				gc.SetColor(255, 240, 200);
				gc.FillRect(0,0,window_w,window_h);
				gc.SetColor(150, 100, 0);
				gc.DrawString("正解！", window_w/2-70, window_h/2-250);
				gc.DrawString(answer, window_w/2-130, window_h/2);
				gc.SetColor(200,0,0);
				gc.FillRect(nextBox_x, nextBox_y, nextBox_w, nextBox_h);
				gc.SetColor(255,255,255);
				gc.DrawString("NEXT", nextBox_x+10, nextBox_y+10);
				// gc.DrawString("Press SpaceKey to continue", window_w/2-170, window_h/2+150);
			}
			//不正解画面
			if(correctFlag == "Dark" || correctFlag == "False"){
				gc.SetColor(150, 150, 255);
				gc.FillRect(0,0,window_w,window_h);
				gc.SetColor(0, 0, 0);
				gc.DrawString("不正解...", window_w/2-100, window_h/2-250);
				gc.DrawString(answer, window_w/2-150, window_h/2);
				gc.SetColor(200,0,0);
				gc.FillRect(nextBox_x, nextBox_y, nextBox_w, nextBox_h);
				gc.SetColor(255,255,255);
				gc.DrawString("NEXT", nextBox_x+10, nextBox_y+10);
			}
		}

	}

	void horrrorUpdate(){

		// Title
		if(gameState==0){
			if(time > tmpTime+100) phase = 4; //so
			else if(time > tmpTime+70) phase = 3; //ko
			else if(time > tmpTime+40) phase = 2; //u
			else if(time > tmpTime+10) phase = 1; //yo

			if(phase == 4){
				gc.PlaySound(GcSound.HorrorBgm, GcSoundTrack.BGM1, true);
				// クリックでスタート
				if(gc.GetPointerFrameCount(0)==1){
					stage += 1;
					phase = 0; 
					tmpTime = time;
					gameState += 1;
				}
			}
		}
		// Thank you for playing...
		else if(gameState == 3){
			if(phase==4 && time > tmpTime+240) phase = 5; 	// murderer move left
			if(phase==3 && time > tmpTime+120){
				phase = 4; 	// murderer step
				gc.PlaySound(GcSound.Mumble, GcSoundTrack.BGM2, false);
			}
			if(phase==2 && time > tmpTime+100) phase = 3; 	// murderer come out
			if(phase==1 && time > tmpTime+50) phase = 2;	// gate open
			if(phase==0 && time > tmpTime+30){	//phase0 の時、扉クリックですすむ
				if((int)pointer_x > 120 && (int)pointer_x < 120+400){
					if((int)pointer_y > door_y && (int)pointer_y < door_y+346){
						tmpTime = time;
						count = window_w/2-60;
						gc.PlaySound(GcSound.HorrorPiano, GcSoundTrack.BGM1, false);
						phase = 1;	// gate half open
					}
				}
			}

			//murderer going left 
			if(phase >= 5 && phase <= 7){
				count -= 2;
				if(time%20 == 0){
					if(step == 0){
						phase += 1;
						if(phase == 7) step = 1;
					}else if(step == 1){
						phase -= 1;
						if(phase == 5) step = 0;
					}
				}
				if(count < -180) phase = 8;
			}

		}
		// END
		else if(gameState == 2){
			if(gc.GetPointerFrameCount(0) > 30){
				phase = 0;
				tmpTime = time;
				gameState = 3;
			}
		}
		// Game Playing
		else if(gameState==1){

			//ドア　クリック判定
			if(door_y < (int)pointer_y && (int)pointer_y < door_y+258 && correctFlag != "False" && correctFlag != "Open"){
				//a_door
				if(a_door_x < (int)pointer_x && (int)pointer_x < a_door_x+150){
					doorFlag = "a";
					if(stage == 2){
						// gc.PlaySound(GcSound.DoorOpen);
						gc.PlaySound(GcSound.DoorOpen, GcSoundTrack.BGM2, false);
						correctFlag = "False";
					}else{
						// gc.PlaySound(GcSound.DoorLocked);
						gc.PlaySound(GcSound.DoorLocked, GcSoundTrack.BGM2, false);
						correctFlag = "Locked";
					}
				}
				//b_door
				if(b_door_x < (int)pointer_x && (int)pointer_x < b_door_x+150){
					doorFlag = "b";
					if(stage == 3){
						gc.StopSound(GcSoundTrack.BGM1);
						tmpTime = time;		// アニメーション用　経過時間一時保存
						correctFlag = "Open";  //これでムービーに入らせたい（操作不可にしたい）
						gc.PlaySound(GcSound.DoorOpen);
					}else{
						// gc.PlaySound(GcSound.DoorLocked);
						gc.PlaySound(GcSound.DoorLocked, GcSoundTrack.BGM2, false);
						correctFlag = "Locked";
					}
				}
				//c_door
				if(c_door_x < (int)pointer_x && (int)pointer_x < c_door_x+150){
					doorFlag = "c";
					if(stage == 1){
						if((time-tmpTime) > 60){
							correctFlag = "False";
							// gc.PlaySound(GcSound.DoorOpen);
							gc.PlaySound(GcSound.DoorOpen, GcSoundTrack.BGM2, false);						
						}
					}else{
						// gc.PlaySound(GcSound.DoorLocked);
						gc.PlaySound(GcSound.DoorLocked, GcSoundTrack.BGM2, false);
						correctFlag = "Locked";
					}
				}
			}
			// クリック後処理(ドアが開くとき)
			if(correctFlag == "False"){
				//「次へ」クリック判定
				if((int)pointer_x > nextBox_x && (int)pointer_x < nextBox_x+nextBox_w){
					if((int)pointer_y > nextBox_y && (int)pointer_y < nextBox_y+nextBox_h){
						stage += 1;
						correctFlag = "0";
						doorFlag = "0";
						pointer_x = 0;
						pointer_y = 0;
						horrorPt += 1;
					}
				}
			}
			if(correctFlag == "Open"){
				// murderer's speed
				if(time == tmpTime+240) count = 0;
				if(time > tmpTime+240) count += 25;

				// animation phase
				if(time > tmpTime+15) phase = 1; // half open door
				if(time > tmpTime+60) phase = 2; // open door
				if(time > tmpTime+120 && time < tmpTime+180) phase = 3; // show murderer's back
				else if(time >= tmpTime+180 && time < tmpTime+240) phase = 4; // murderer turns around
				else if(time >= tmpTime+ 240 && door_y+10+count < player_y-190) {
					phase = 5; // murderer comes close to the bear
					// if(door_y+10+count > player_y-200) horrorPt = 3;
				}
				else if(time >= tmpTime+260) {
					phase = 6; // red out, white noise
					gc.PlaySound(GcSound.WhiteNoise, GcSoundTrack.BGM1, true);
				}
				if(time > tmpTime+380){ //go to the end
					gc.StopSound(GcSoundTrack.BGM1);
					gameState += 1;
				}
			}

		}
	}

	void horrorDraw(){
		gc.ClearScreen();

		//Title
		if(gameState == 0){
			gc.SetBackgroundColor(0,0,0);
			gc.SetFontSize(200);
			gc.SetColor(255,0,0);
			if(phase >= 1) gc.DrawString("よ", window_w/2, 0);
			if(phase >= 2) gc.DrawString("う", window_w/2-220, window_h/2-200);
			if(phase >= 3) gc.DrawString("こ", window_w/2+50, window_h/2+50);
			if(phase >= 4) gc.DrawString("そ", window_w/2-150, window_h-200);
		}
		// Thank you for Playing
		else if(gameState == 3){
			gc.SetBackgroundColor(0,0,0);
			if(phase == 0) {
				gc.SetFontSize(50);
				gc.SetColor(255,0,0);
				gc.DrawString("谿ｺ莠ｺ鬯ｼとおべんきょう", window_w/2-280, 150);
				gc.DrawImage(gate_closed, 120, door_y);
			}
			if(phase >= 1) gc.DrawImage(GcImage.Hr_gate_halfOpen, 120, door_y);
			if(phase >= 2) gc.DrawImage(GcImage.Hr_gate_open, 120, door_y);

			//murderer animation
			if(phase == 3) gc.DrawImage(GcImage.Hr_murderer_front2, window_w/2-60, door_y+146);
			else if(phase == 4) gc.DrawImage(murder_front, window_w/2-60, door_y+146);
			else if(phase == 5) gc.DrawImage(murder_side, count, door_y+146);	//side1
			else if(phase == 6) gc.DrawImage(GcImage.Hr_murderer_side2, count, door_y+146);	//side2
			else if(phase == 7) gc.DrawImage(GcImage.Hr_murderer_side3, count, door_y+146);	//side3

			if(phase == 8) {
				gc.SetFontSize(50);
				gc.DrawString("Thank you for playing...", 20, window_h-200);
			}

		}
		// End
		else if(gameState == 2){
			gc.SetBackgroundColor(255,0,0);			
			gc.SetColor(0,0,0);
			gc.SetFontSize(150);
			gc.DrawString("END", window_w/2-100, window_h/2-300);
			gc.SetFontSize(30);
			gc.DrawString("長　押し　で　タイト　ルへ", window_w/2-200, window_h-100);
			// gc.DrawImage(GcImage.Hr_bear, player_x, window_h/2);
			if(slowRism > 0) gc.DrawImage(GcImage.Hr_bear_big, player_x, window_h/2+100);
			else if (slowRism < 0) gc.DrawImage(GcImage.Hr_bear_big, player_x, window_h/2+100);			
		}
		//Game Playing
		else if(gameState == 1){
			gc.SetBackgroundColor(0,0,0);
			gc.SetFontSize(30);
			// gc.DrawString("stage "+stage, 5, 0);
			gc.SetFontSize(50);

			// クマの画像上下（速度はrism参照）
			if(horrorPt < 3){  //normal ver.
				if(rism > 0) gc.DrawImage(player_img, player_x, player_y);
				else if (rism < 0) gc.DrawImage(player_img, player_x, player_y+5);
			}
			
			// doors animation
			gc.DrawImage(GcImage.Hr_bDoor_closed, a_door_x, door_y);
			gc.DrawImage(GcImage.Hr_bDoor_closed, b_door_x, door_y);
			gc.DrawImage(GcImage.Hr_bDoor_closed, c_door_x, door_y);

			// questions
			if(stage == 1){
				question = " 1 + 2 + 3 + 4 = ?";
				answer_a = "10"; //correct
				answer_b = "11";
				answer_c = "-8";　//open
				answer = "";
			} else if(stage == 2){
				question = "175 ÷ 7 x 25 = ?";
				answer_a = "49"; //open
				answer_b = "525";
				answer_c = "625"; //correct
			} else if(stage == 3){
				question = "君　は　今　独り?";
				answer_a = "いいえ";
				answer_b = "はい";  //open
				answer_c = "いいえ";
			}

			// 問題描画
			gc.SetColor(255,0,0);
			gc.DrawString(question, window_w/2-200, 120); //question
			if(stage==3) gc.SetFontSize(30);
			gc.DrawString(answer_a, a_door_x+10, door_y-50);	//a
			gc.DrawString(answer_b, b_door_x+10, door_y-50);	//b
			gc.DrawString(answer_c, c_door_x+10, door_y-50);	//c

			
			if(correctFlag == "False"){
				if(doorFlag=="a") gc.DrawImage(GcImage.Hr_bbDoor_open, a_door_x, door_y);
				else if(doorFlag=="b") gc.DrawImage(GcImage.Hr_bbDoor_open, b_door_x, door_y);
				else if(doorFlag=="c") gc.DrawImage(GcImage.Hr_bbDoor_open, c_door_x, door_y);
				gc.SetColor(0,0,0);
				gc.FillRect(0, window_h/2+100, window_w, window_h);	// クマ隠し
				gc.SetColor(255,0,0);
				gc.FillRect(nextBox_x, nextBox_y, nextBox_w, nextBox_h);
				gc.SetColor(0,0,0);
				gc.DrawString("次へ", nextBox_x+10, nextBox_y+5);
			}else if(correctFlag == "Open"){
				gc.SetColor(0,0,0);
				gc.FillRect(0, 0, window_w, window_h/2+100);  //文字消し	
				gc.DrawImage(GcImage.Hr_bDoor_closed, b_door_x, door_y);
				
				//アニメーション
				if(phase >= 1) gc.DrawImage(GcImage.Hr_bbDoor_halfOpen, b_door_x, door_y);
				if(phase >= 2) gc.DrawImage(GcImage.Hr_bbDoor_open, b_door_x, door_y);
				

				if(phase == 3) gc.DrawImage(murder_back, window_w/2-60, door_y+10);
				else if(phase == 4) gc.DrawImage(murder_front, window_w/2-60, door_y+10);
				else if(phase == 5) gc.DrawImage(murder_front, window_w/2-60, door_y+10+count);
				else if(phase == 6){
					// gc.PlaySound(GcSound.WhiteNoise, GcSoundTrack.BGM1, true);
					gc.SetColor(255,0,0);
					gc.FillRect(0,0,window_w,window_h);
				}
			}
			

		}


	}

  
}
