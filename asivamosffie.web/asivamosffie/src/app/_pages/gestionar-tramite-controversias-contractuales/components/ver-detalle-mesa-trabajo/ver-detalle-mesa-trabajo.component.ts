import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalle-mesa-trabajo',
  templateUrl: './ver-detalle-mesa-trabajo.component.html',
  styleUrls: ['./ver-detalle-mesa-trabajo.component.scss']
})
export class VerDetalleMesaTrabajoComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
