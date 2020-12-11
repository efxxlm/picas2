import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-registrar-nueva-mesatrabajo-act',
  templateUrl: './registrar-nueva-mesatrabajo-act.component.html',
  styleUrls: ['./registrar-nueva-mesatrabajo-act.component.scss']
})
export class RegistrarNuevaMesatrabajoActComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
