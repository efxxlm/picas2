import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-validar-balance',
  templateUrl: './validar-balance.component.html',
  styleUrls: ['./validar-balance.component.scss']
})
export class ValidarBalanceComponent implements OnInit {

  constructor(
    private routes: Router
  ) { }

  ngOnInit(): void {
  }

  
}
