import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-actualizar-mesa-de-trabajo',
  templateUrl: './actualizar-mesa-de-trabajo.component.html',
  styleUrls: ['./actualizar-mesa-de-trabajo.component.scss']
})
export class ActualizarMesaDeTrabajoComponent implements OnInit {

  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
