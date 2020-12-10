import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-actualizar-reclamacion-aseg-cc',
  templateUrl: './actualizar-reclamacion-aseg-cc.component.html',
  styleUrls: ['./actualizar-reclamacion-aseg-cc.component.scss']
})
export class ActualizarReclamacionAsegCcComponent implements OnInit {
  idControversia: any;

  

  tipoControversia: string;
  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.tipoControversia="Terminación anticipada por incumplimiento (TAI)";
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
