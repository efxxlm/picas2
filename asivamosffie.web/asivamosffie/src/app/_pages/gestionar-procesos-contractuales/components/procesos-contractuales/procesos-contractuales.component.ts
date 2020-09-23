import { Component, OnInit } from '@angular/core';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { GrillaProcesosContractuales } from '../../../../_interfaces/procesosContractuales.interface';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-procesos-contractuales',
  templateUrl: './procesos-contractuales.component.html',
  styleUrls: ['./procesos-contractuales.component.scss']
})
export class ProcesosContractualesComponent implements OnInit {

  verAyuda = false;
  $data: Observable<GrillaProcesosContractuales[]>;
  estadoAcordeon: string;

  constructor ( private procesosContractualesSvc: ProcesosContractualesService ) {
    this.$data = this.procesosContractualesSvc.getGrilla();
  };

  ngOnInit () {
  };

};