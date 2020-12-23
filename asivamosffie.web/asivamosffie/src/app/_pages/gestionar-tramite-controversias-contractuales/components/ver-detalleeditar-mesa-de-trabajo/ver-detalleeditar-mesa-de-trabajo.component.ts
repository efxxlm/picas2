import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-mesa-de-trabajo',
  templateUrl: './ver-detalleeditar-mesa-de-trabajo.component.html',
  styleUrls: ['./ver-detalleeditar-mesa-de-trabajo.component.scss']
})
export class VerDetalleeditarMesaDeTrabajoComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
