import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-registrar-nueva-mesatrabajo-cc',
  templateUrl: './registrar-nueva-mesatrabajo-cc.component.html',
  styleUrls: ['./registrar-nueva-mesatrabajo-cc.component.scss']
})
export class RegistrarNuevaMesatrabajoCcComponent implements OnInit {

  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
