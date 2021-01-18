import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-actuacion-mt',
  templateUrl: './ver-detalleeditar-actuacion-mt.component.html',
  styleUrls: ['./ver-detalleeditar-actuacion-mt.component.scss']
})
export class VerDetalleeditarActuacionMtComponent implements OnInit {

  idSeguimientoMesa: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idSeguimientoMesa = param.id;
    });
  }

}
