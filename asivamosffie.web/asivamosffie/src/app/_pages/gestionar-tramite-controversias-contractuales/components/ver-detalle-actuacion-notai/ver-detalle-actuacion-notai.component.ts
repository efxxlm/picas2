import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalle-actuacion-notai',
  templateUrl: './ver-detalle-actuacion-notai.component.html',
  styleUrls: ['./ver-detalle-actuacion-notai.component.scss']
})
export class VerDetalleActuacionNotaiComponent implements OnInit {

  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
