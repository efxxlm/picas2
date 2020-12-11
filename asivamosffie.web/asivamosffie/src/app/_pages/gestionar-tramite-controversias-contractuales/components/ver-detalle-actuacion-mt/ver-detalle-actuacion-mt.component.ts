import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalle-actuacion-mt',
  templateUrl: './ver-detalle-actuacion-mt.component.html',
  styleUrls: ['./ver-detalle-actuacion-mt.component.scss']
})
export class VerDetalleActuacionMtComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }
}
