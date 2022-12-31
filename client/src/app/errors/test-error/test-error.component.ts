import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  validationsErrors: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void{}

  getError(){
    this.http.get(this.baseUrl + 'users/-1').subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error);
        this.validationsErrors = error
      }
    })
  }
}
