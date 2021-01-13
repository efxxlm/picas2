import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-reclamacion',
  templateUrl: './ver-detalleeditar-reclamacion.component.html',
  styleUrls: ['./ver-detalleeditar-reclamacion.component.scss']
})
export class VerDetalleeditarReclamacionComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
