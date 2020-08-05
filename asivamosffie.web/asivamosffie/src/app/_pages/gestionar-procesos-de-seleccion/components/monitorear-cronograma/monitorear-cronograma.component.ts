import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-monitorear-cronograma',
  templateUrl: './monitorear-cronograma.component.html',
  styleUrls: ['./monitorear-cronograma.component.scss']
})
export class MonitorearCronogramaComponent implements OnInit {

  verAyuda = false;

  ajustarCronograma = false;
  registrarSeguimiento = false;

  constructor() { }

  ngOnInit(): void {
  }

}
