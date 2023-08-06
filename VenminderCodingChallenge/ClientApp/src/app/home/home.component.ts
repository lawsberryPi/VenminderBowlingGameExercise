import { Inject, Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import axios from 'axios';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public rollScore: string | undefined;
  public bowlingResponse: BowlingScoreResponse | undefined;
  public totalScore: number | undefined;

  private _httpClient: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._httpClient = http;
    this._baseUrl = baseUrl;
  }

  /**
 * reset the total score from server
 * */
  public deleteTotalScore() {

    //const option: 
    this._httpClient.delete<boolean>(this._baseUrl + 'bowling').subscribe(result => {
      this.rollScore = undefined;
      this.bowlingResponse = undefined;
      this.totalScore = undefined;
    }, error => console.error(error));

  }

  /**
   * get the total score from server
   * */
  public getTotalScore() {

    //const option: 
    this._httpClient.get<number>(this._baseUrl + 'bowling').subscribe(result => {
      this.totalScore = result;
    }, error => console.error(error));
    
  }

  /**
   * Post the roll score to the server 
   * */
  public postRollScore() {
    const rollInfo = this.bowlingResponse?.nextRoll;
    const intScore = parseInt(this.rollScore ?? "0");
    if (intScore > 10) {
      alert("you can not hit more than 10 pins for one delivery");
      return
    };
    const roll1Score = this.bowlingResponse?.framesData[this.bowlingResponse?.framesData.length - 1 ]?.roll1 ?? 0;
    if (rollInfo?.rollNumber == 2 && (roll1Score + intScore) > 10 && rollInfo?.frameNumber !== 10) {
      alert("you can not hit more than 10 pins for one frame");
      return
    }
    const formData = new FormData();
    // because for first roll in first frame, there will be no bowlingResponse, therefore assign 1
    formData.append("frameNumber", rollInfo?.frameNumber.toString() ?? "1");
    formData.append("rollNumber", rollInfo?.rollNumber.toString() ?? "1");
    formData.append("rollScore", this.rollScore ?? "0");

    this._httpClient.post<BowlingScoreResponse>(this._baseUrl + 'bowling', formData).subscribe(result => {
      this.bowlingResponse = result;
//      this.rollScore = undefined;
    }, error => console.error(error));

  }
}

interface BowlingScoreResponse {
  framesData: FrameDetail[];
  nextRoll: NextRoll;
}

interface FrameDetail {
  frameNumber: number,
  roll1: number | null,
  roll2: number | null,
  roll3: number | null,
  frameScore: number
}

interface NextRoll {
  frameNumber: number,
  rollNumber: number,
  isEndOfGame: boolean
}

