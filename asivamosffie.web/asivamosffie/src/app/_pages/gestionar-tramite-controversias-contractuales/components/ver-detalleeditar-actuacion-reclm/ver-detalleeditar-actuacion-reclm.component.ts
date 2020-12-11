import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-actuacion-reclm',
  templateUrl: './ver-detalleeditar-actuacion-reclm.component.html',
  styleUrls: ['./ver-detalleeditar-actuacion-reclm.component.scss']
})
export class VerDetalleeditarActuacionReclmComponent implements OnInit {
  
  idControversia: any;
  idReclamacionActuacion:any;
  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idReclamacionActuacion = param.id;
    });
  }

}
