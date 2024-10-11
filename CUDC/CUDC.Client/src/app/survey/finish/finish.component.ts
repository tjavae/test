import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-finish',
  templateUrl: './finish.component.html',
  styleUrls: ['./finish.component.scss']
})
export class FinishComponent implements OnInit {
  titleMessage: string;
  displayMessage: string;

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.titleMessage = 'Thank you for taking the time to complete our survey.';
    this.displayMessage = 'Your data was submitted successfully!';
  }

}
