import { Component, Input, OnInit } from '@angular/core';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-recibo-satisfaccion',
  templateUrl: './recibo-satisfaccion.component.html',
  styleUrls: ['./recibo-satisfaccion.component.scss']
})
export class ReciboSatisfaccionComponent implements OnInit {

  @Input() report: Report

  constructor() { }

  ngOnInit(): void {
  }

}
