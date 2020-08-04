import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-registrar-informacion-adicional',
  templateUrl: './registrar-informacion-adicional.component.html',
  styleUrls: ['./registrar-informacion-adicional.component.scss']
})
export class RegistrarInformacionAdicionalComponent implements OnInit {

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      console.log(params);
      const id = params.id;
      // const a = this.service.getService(id;
      // console.log(a);
    });
  }

}
