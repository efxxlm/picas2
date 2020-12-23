import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalleeditar-actuacion-notai',
  templateUrl: './ver-detalleeditar-actuacion-notai.component.html',
  styleUrls: ['./ver-detalleeditar-actuacion-notai.component.scss']
})
export class VerDetalleeditarActuacionNotaiComponent implements OnInit {

  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }
}
