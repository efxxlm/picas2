import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-registar-reclamacion-aseguradora-cc',
  templateUrl: './registar-reclamacion-aseguradora-cc.component.html',
  styleUrls: ['./registar-reclamacion-aseguradora-cc.component.scss']
})
export class RegistarReclamacionAseguradoraCcComponent implements OnInit {
  idControversia: any;
  idActuacion: any;
  actuacion = localStorage.getItem("actuacion");
  numActuacion = localStorage.getItem("numeroActuacion"); 
  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.idControversia;
      this.idActuacion = param.id;
    });
  }

}
