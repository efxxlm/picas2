import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-actualizar-reclamacion-aseg-cc',
  templateUrl: './actualizar-reclamacion-aseg-cc.component.html',
  styleUrls: ['./actualizar-reclamacion-aseg-cc.component.scss']
})
export class ActualizarReclamacionAsegCcComponent implements OnInit {
  tipoControversia: string;

  constructor() { }

  ngOnInit(): void {
    this.tipoControversia="Terminación anticipada por incumplimiento (TAI)";
  }

}
