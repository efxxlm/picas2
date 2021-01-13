import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-mt-actuacion',
  templateUrl: './ver-detalleeditar-mt-actuacion.component.html',
  styleUrls: ['./ver-detalleeditar-mt-actuacion.component.scss']
})
export class VerDetalleeditarMtActuacionComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }
}
