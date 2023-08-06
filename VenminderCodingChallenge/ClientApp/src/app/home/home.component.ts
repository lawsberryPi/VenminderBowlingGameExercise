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
    //const option:
    const formData = new FormData();
    // because for first roll in first frame, there will be no bowlingResponse, therefore assign 1
    formData.append("frameNumber", this.bowlingResponse?.nextRoll.frameNumber.toString() ?? "1");
    formData.append("rollNumber", this.bowlingResponse?.nextRoll.rollNumber.toString() ?? "1");
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

