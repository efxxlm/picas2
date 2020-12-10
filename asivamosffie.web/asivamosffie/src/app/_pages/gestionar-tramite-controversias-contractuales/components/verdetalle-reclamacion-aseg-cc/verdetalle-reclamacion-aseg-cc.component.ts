import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-verdetalle-reclamacion-aseg-cc',
  templateUrl: './verdetalle-reclamacion-aseg-cc.component.html',
  styleUrls: ['./verdetalle-reclamacion-aseg-cc.component.scss']
})
export class VerdetalleReclamacionAsegCcComponent implements OnInit {
  idControversia: any;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }

}
